using Coursewise.Common.Models;
using Coursewise.Security.Compact.Interfaces;
using Coursewise.Security.Compact.Models;
using Coursewise.Security.Compact.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddIdentity<CoursewiseUser, IdentityRole>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            });


            // Authentication Bearer Token
            services.AddTransient<ITokenGenerator, TokenGeneratorService>();
            services.Configure<TokenProviderOptions>(options => configuration.GetSection("Nova:TokenProviderOptions").Bind(options));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Nova:TokenProviderOptions:secret"]));
            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Nova:TokenProviderOptions:issuer"],

                ValidateAudience = true,
                ValidAudience = configuration["Nova:TokenProviderOptions:audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCreds.Key,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)

            };
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParams;
                });
        }
    }
}
