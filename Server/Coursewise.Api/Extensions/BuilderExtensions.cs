using Coursewise.Api.Utility;
using Coursewise.Common.Utilities;
using Coursewise.Logging;
using Coursewise.Module.Extensions;
using Coursewise.Typeform.Extensions;
using Gelf.Extensions.Logging;

namespace Coursewise.Api.Extensions
{
    public static class BuilderExtensions
    {
        public static void SetBuilder(this WebApplicationBuilder builder)
        {
            string environment = builder.Configuration["Logging:GELF:AdditionalFields:Environment"];
            builder.Environment.EnvironmentName = AssignEnvironment(environment);

            builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("emailTemplates.json",
                                   optional: true,
                                   reloadOnChange: true);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            builder.Services.AddAuthorizationPolicies();
            builder.Services.Configure<AWS.AwsSettings>(
                builder.Configuration.GetSection("AWS"));
            builder.Services.Configure<Typeform.Models.TypeformConfig>(
                builder.Configuration.GetSection("TypeformConfig"));
            builder.Services.Configure<Domain.Models.AppOptions>(
                builder.Configuration.GetSection("Nova"));
            builder.Services.Configure<Domain.Models.WordpressOptions>(
                builder.Configuration.GetSection("Wordpress"));

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
                .AddGelf(configure =>
                {
                    configure.AdditionalFields["machine_name"] = Environment.MachineName;
                });
            builder.Services.AddScoped(typeof(ICoursewiseLogger<>), typeof(GrayLoggerService<>));
            builder.Services.AddScoped<Strategies.Interfaces.ILoginStrategyExecuter, Strategies.Services.LoginStrategy>();

            #region DirectModules

            builder.Services.AddTypeform();

            #endregion DirectModules

            #region CommonModules

            builder.Services.AddCoursewiseServices(builder.Configuration);

            #endregion CommonModules
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(CoursewiseRoles.ADMIN, policy => policy.RequireClaim(ClaimValues.Role, CoursewiseRoles.ADMIN));
                options.AddPolicy(CoursewiseRoles.CUSTOMER, policy => policy.RequireClaim(ClaimValues.Role, CoursewiseRoles.CUSTOMER));

            });
        }

        private static string AssignEnvironment(string environment)
        {
            string value = environment.ToLowerInvariant();
            if (value == "quality")
            {
                return "Quality";
            }
            if (value == "staging")
            {
                return "Staging";
            }
            if (value == "development" || value == "local")
            {
                return "Development";
            }
            return "Production";
        }
    }
}