using Coursewise.Data;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Data.Repositories;
using Coursewise.Security.Compact.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Module.DependencyInjection
{
    public static class RepositoryModule
    {
        public static void AddRepositoryServices(this IServiceCollection services, IConfiguration config)
        {


            var connectionString = config.GetConnectionString("Default");
            services.AddDbContext<CoursewiseDbContext>(
                options => options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    mySqlServerOptions => mySqlServerOptions.MigrationsAssembly("Coursewise.Data")
                    ));

            services.AddScoped<ICoursewiseDbContext, CoursewiseDbContext>();
            services.AddScoped<IUnitOfWork>(unitOfWork => new UnitOfWork(unitOfWork.GetService<ICoursewiseDbContext>()!));
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IServiceStatusRepository, ServiceStatusRepository>();

            services.AddScoped<IArtifactRepository,ArtifactRepository >();
            services.AddScoped <IIndustryRepository, IndustryRepository > ();
            services.AddScoped <ILocationRepository, LocationRepository> ();
            services.AddScoped <ISkillRepository, SkillRepository> ();
            services.AddScoped<IApplyJobRepository, ApplyJobRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();

            services.AddIdentity<CoursewiseDbContext>(config);
        }
    }
}
