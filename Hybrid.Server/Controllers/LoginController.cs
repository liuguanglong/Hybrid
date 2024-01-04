using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Supabase.Gotrue.Interfaces;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;

namespace Hybrid.Server.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IGotrueAdminClient<User> _client;
        private readonly IConfiguration Configuration;

        private readonly String ServiceKey;
        public LoginController(
            IGotrueAdminClient<User> client,
            IConfiguration configuration)
        {
            _client = client;
            Configuration = configuration;
        }

        [HttpPost(Name = "login")]
        public async Task<IActionResult> Login([FromBody]String jwt)
        {
            var user = await _client.GetUser(jwt);
            if (user == null) {
                return Unauthorized();
            }

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            JArray userRoles = (JArray)user.AppMetadata["role"];
            foreach (var userRole in userRoles.Children())
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Value<String>()));
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
