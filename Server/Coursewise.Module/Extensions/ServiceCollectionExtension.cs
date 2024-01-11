using Coursewise.Data;
using Coursewise.Data.Extensions;
using Coursewise.Module.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coursewise.Module.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoursewiseServices(
            this IServiceCollection services,
            IConfiguration configuration,
            AutoMapper.MapperConfigurationExpression? mapperConfigurationExpression = null)
        {
            services.AddRepositoryServices(configuration);
            services.AddBusinessServices();
            services.AddScheduling(configuration);
            services.AddCoursewiseComponent(mapperConfigurationExpression);
            return services;
        }

        public static async Task UseAutoMigration(this IServiceProvider serviceProvider, string env, IConfiguration configuration)
        {
            serviceProvider.UseAutoMigration<CoursewiseDbContext>();
            await SeedCoursewiseData.SeedAllData(serviceProvider, env, configuration);
        }
    }
}