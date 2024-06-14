using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Services;
using JiraLikeSystem.Data;
using JiraLikeSystem.Models.Users;
using JiraLikeSystem.WebApi.Authentication.JwtToken;
using JiraLikeSytem.Domain.Models.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JiraLikeSystem;

public static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        //============ DB Context =============
        builder.Services.AddDbContext<JiraLikeDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
      .AddEntityFrameworkStores<JiraLikeDbContext>()
      .AddDefaultTokenProviders();


        //============ Scopes =============
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

        //============ Scopes =============
        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        //============ Authentication(JWT Bearer) =============
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        //============ Authorization=============
        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        //============ Roles =============

        //InitializeRoles(builder.Services).Wait();
        InitializeRoles(builder.Services.BuildServiceProvider()).GetAwaiter().GetResult();

        //============ Swagger =============
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Jira Like System", Version = "v1" });

            //============ Jwt Settings =============
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }

    private static async Task InitializeRoles(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string[] roleNames = { "Admin", "Project Manager", "Developer", "Tester" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }
        }
    }
}
