using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Supabase.Client client = new Supabase.Client(
        builder.Configuration["SupaServer:Url"],
        builder.Configuration["SupaServer:Key"],
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }
);

await client.Auth.SignIn(builder.Configuration["SupaServer:ServiceAccount"], builder.Configuration["SupaServer:ServicePassword"]);
IGotrueAdminClient<User> amdinClient = client.AdminAuth(builder.Configuration["SupaServer:Key"]);

builder.Services.AddScoped<Supabase.Client>(
   provider => client
);

builder.Services.AddScoped<IGotrueAdminClient<User>>(
   provider => amdinClient
);

var iss = builder.Configuration["JWT:ValidIssuer"].ToString();
var key = builder.Configuration["JWT:Secret"].ToString();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidAudience = "authenticated",
                        ValidIssuer = iss,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true                        
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = context =>
                        {
                            // Find claim identity attached to principal.
                            var claim = (Claim)context.Principal.Claims.Where(x=>x.Type=="app_metadata").FirstOrDefault();
                            JObject role = JObject.Parse(claim.Value);
                            JArray roles = (JArray)role.GetValue("role");
                            var claims = new List<Claim>();
                            foreach (var r in roles)
                                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Value<String>()));
                            var appIdentity = new ClaimsIdentity(claims);
                            context.Principal.AddIdentity(appIdentity);

                            return Task.CompletedTask;
                        }
                    }
                    ;
                }
              );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
