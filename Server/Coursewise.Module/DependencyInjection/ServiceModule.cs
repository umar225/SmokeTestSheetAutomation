using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Jobs;
using Coursewise.Domain.Services;
using Coursewise.Domain.Services.Facade;
using Coursewise.Security.Compact.Interfaces;
using Coursewise.Security.Compact.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Coursewise.Module.DependencyInjection
{
    public static class ServiceModule
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<ICoursewiseSecurityService, CoursewiseSecurityService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IExternalLoginService, ExternalLoginService>();
            services.AddScoped<Domain.Interfaces.External.IWordpressService, Domain.Services.External.WordpressService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IArtifactService, ArtifactService>();
            services.AddScoped <IIndustryService, IndustryService> ();
            services.AddScoped <ILocationService, LocationService> ();
            services.AddScoped <ISkillService, SkillService> ();
            services.AddScoped<IApplyJobService, ApplyJobService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<ITitleService, TitleService>();
            services.AddScoped<IJobEmailService, JobEmailService>();

            #region Facade
            services.AddScoped<IUtilityFacade, UtilityFacade>();
            services.AddScoped<IAwsServiceFacade, AwsServiceFacade>();
            services.AddScoped<IJobSelectionFacade, JobSelectionFacade>();
            services.AddScoped<IJobServiceFacade, JobServiceFacade>();
            services.AddScoped<IApplyJobServiceFacade, ApplyJobServiceFacade>();
            services.AddScoped<IResourceServiceFacade, ResourceServiceFacade>();

            #endregion
        }

        public static void AddScheduling(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IJobServiceContainer<>), typeof(JobServiceContainer<>));
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.ScheduleJob<SendJobMailJob>(trigger => trigger
                    .WithIdentity("SendJobMailJob")
                    .WithCronSchedule(config["BackgroundJobs:SendJobMailJob"])
                    .WithDescription("This trigger will run every half an hour to send emails about jobs.")
                );
            });

            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }
    }
}