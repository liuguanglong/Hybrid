using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System;
using System.Text;

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

builder.Services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
                        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
                        // When receiving a token, check that we've signed it.
                        ValidateIssuerSigningKey = true,
                        // When receiving a token, check that it is still valid.
                        ValidateLifetime = true
                    };
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
