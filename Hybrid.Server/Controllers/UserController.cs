using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;

namespace Hybrid.Server.Controllers
{

    [Authorize(Roles = "Manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGotrueAdminClient<User> _client;

        public UserController(
            IGotrueAdminClient<User> client)
        {
            this._client = client;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<UserList<User>> Get()
        {
            //await _client.Auth.SignIn("liuguanglong@gmail.com", "AAbbcc!!0");
            var users = await _client.ListUsers();
            return users;
        }
    }
}
