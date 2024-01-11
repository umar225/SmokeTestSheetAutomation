using Microsoft.Extensions.DependencyInjection;

namespace Coursewise.Typeform.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTypeform(this IServiceCollection services)
        {
            services.AddHttpClient();
        }
    }
}