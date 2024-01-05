using Hybrid.Service.Shared.Data;
using Hybrid.Service.Shared.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System.Data;
using System.Security.Claims;

namespace Hybrid.Server.Controllers
{
    [Authorize(Roles = "Manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserService
    {
        private readonly IGotrueAdminClient<User> _client;

        public UserController(
            IGotrueAdminClient<User> client)
        {
            this._client = client;
        }

        [HttpGet("GetUsers")]
        public async Task<UserInfo[]?> GeUsersAsync()
        {
            var users = await _client.ListUsers();
            List<UserInfo> result = new List<UserInfo>();
            foreach (var usr in users.Users)
            {
                UserInfo userInfo = new UserInfo { Id = usr.Id, Email = usr.Email, CreatedAt = usr.CreatedAt };
                JArray roles = (JArray)usr.AppMetadata["role"];
                List<String> listrole = new List<String>();
                foreach (var r in roles)
                    listrole.Add( r.Value<String>());
                userInfo.Roles = listrole.ToArray();

                result.Add(userInfo);
            }
            return result.ToArray();
        }

        [HttpPost("UpdateRoles")]
        public async Task UpdateRolesAsync([FromBody]UserInfo userInfo)
        {
            var user = await _client.GetUserById(userInfo.Id);
            if(user != null)
            {
                AdminUserAttributes attributes = new AdminUserAttributes();
                JArray role = new JArray();
                foreach (var r in userInfo.Roles)
                    role.Add(r);

                attributes.AppMetadata["role"] = role;
                await _client.UpdateUserById(userInfo.Id, attributes);
            }
        }
    }
}
