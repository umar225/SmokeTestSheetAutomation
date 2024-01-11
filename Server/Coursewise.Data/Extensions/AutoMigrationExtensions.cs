using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;

namespace Coursewise.Data.Extensions
{
    public static class AutoMigrationExtensions
    {
        /// <summary>
        /// Use to Apply Auto migrations and data seedings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <param name="seed"></param>
        public static void UseAutoMigration<T>(this IServiceProvider serviceProvider, Action<T>? seed = null) where T : DbContext
        {
            using (var scope = serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<T>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                if (seed == null) return;
                seed.Invoke(context);
                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
            }
        }
    }
}
