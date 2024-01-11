using Coursewise.AWS.Communication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAWS(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPersistenceService, S3PersistenceService>();
        }
    }
}
