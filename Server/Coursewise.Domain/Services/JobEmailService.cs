using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Models;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class JobEmailService : GenericService<Data.Entities.Job, Job, Guid>, IJobEmailService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IServiceStatusRepository _serviceStatusRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly WordpressOptions _wordpressOptions;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoursewiseLogger<JobEmailService> _logger;

        public JobEmailService(IJobRepository jobRepository,
             IResourceRepository resourceRepository,
        ICustomerRepository customerRepository,
            IServiceStatusRepository serviceStatusRepository,
            IEmailTemplateService emailTemplateService,
            IUtilityFacade utilityFacade,
            IWebHostEnvironment hostingEnvironment,
            IOptionsSnapshot<WordpressOptions> _wordpressOptionsSnapShot,
            ICoursewiseLogger<JobEmailService> logger
            ) : base(utilityFacade.Mapper, jobRepository, utilityFacade.UnitOfWork)
        {
            _jobRepository = jobRepository;
            _resourceRepository = resourceRepository;
            _customerRepository = customerRepository;
            _wordpressOptions = _wordpressOptionsSnapShot.Value;
            _serviceStatusRepository = serviceStatusRepository;
            _emailTemplateService = emailTemplateService;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = utilityFacade.UnitOfWork;
            _logger = logger;
        }
        public async Task<BaseModel> SendEmailToCustomers(Guid jobId)
        {
           
            var job = await _jobRepository.Get()
                .Include(j => j.JobTitles)
                    .ThenInclude(x => x.Title)
                .FirstOrDefaultAsync(j => j.Id==jobId && j.IsVisible && !j.IsClosed && !j.IsEmailSent);
            if (job is null)
            {
             
                return BaseModel.Error("Job does not exist, or notificaton already sent");
            }
            var jobs =new List<Data.Entities.Job>() { job };
            var response =await SendJobEmailToCustomers(jobs);
            if (!response.success)
            {
                return response;
            }
            job.IsEmailSent = true;
            await _jobRepository.UpdateAsync(job);
            await _unitOfWork.SaveChangesAsync();
            return response;
        }
       
        public async Task<BaseModel> SendEmailToCustomers()
        {
            var notificationService = await _serviceStatusRepository.FirstOrDefaultAsync(a => a.Title == "Notifications");
            if (notificationService != null)
            {
                if (notificationService.IsRunning)
                {
                    return BaseModel.Error("Notification Service is already running.");

                }
                else
                {
                    notificationService.IsRunning = true;
                    await _serviceStatusRepository.UpdateAsync(notificationService);
                }
            }
            var jobs = await _jobRepository.Get()
                .Where(j => j.IsVisible && !j.IsClosed && !j.IsEmailSent)
                .Include(j=>j.JobTitles)
                    .ThenInclude(x => x.Title)
                .ToListAsync();
            var resources=await _resourceRepository.Get().Where(a => a.IsReady && !a.isDeleted && !a.IsEmailSent).ToListAsync();
            var jobResponse = await SendJobEmailToCustomers(jobs);


            if (!jobResponse.success)
            {
               await ResetNotificationServiceStatus();
                return jobResponse;
            }
            else
            {
                jobs.ForEach(j => j.IsEmailSent = true);
                await _jobRepository.UpdateAsync(jobs);
                await _unitOfWork.SaveChangesAsync();
                _logger.Info($"Following jobs {String.Join(",", jobs.Select(s => s.Id))} Email sent status updated");

            }
            var resourcesResponse = await SendResourcesEmailToCustomers(resources);
            if (!resourcesResponse.success)
            {
                await ResetNotificationServiceStatus();
                return resourcesResponse;
            }
            else
            {
                resources.ForEach(j => j.IsEmailSent = true);
                await _resourceRepository.UpdateAsync(resources);
                await _unitOfWork.SaveChangesAsync();
                _logger.Info($"Following resources {String.Join(",", resources.Select(s => s.Id))} Email sent status updated");

            }
            await ResetNotificationServiceStatus();
            return resourcesResponse;
        }
        private async Task ResetNotificationServiceStatus()
        {
            var notificationServiceStatus = await _serviceStatusRepository.FirstOrDefaultAsync(a => a.Title == "Notifications");
            if (notificationServiceStatus != null)
            {

                notificationServiceStatus.IsRunning = false;
                  await _serviceStatusRepository.UpdateAsync(notificationServiceStatus);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task<BaseModel> SendJobEmailToCustomers(List<Data.Entities.Job> jobs)
        {
            if (jobs.Any())
            {
              
                
                var users =await  _customerRepository.Get().Include(a => a.CoursewiseUser).Where(a => a.CoursewiseUser != null && a.CoursewiseUser.JobNotification).ToListAsync();
                
                if (_wordpressOptions.IsSendEmail)
                {
                    if (_hostingEnvironment.EnvironmentName != "Production")
                    {
                        var onlyTheseEmails = new List<string>() { "@wearenova.co.uk","@seventechnology.co.uk", "@fusiontech.global", "hassankhan87@hotmail.com"};
                        users = users.Where(e=> onlyTheseEmails.Exists(x=>e.CoursewiseUser.Email.ToLower().Contains(x))).ToList();
                    }
                    // send notification
                    foreach (var job in jobs)
                    {
                        await _emailTemplateService.JobOpportunity(users, job);
                    }
                    _logger.Info($"{jobs.Count} jobs email notifications have been sent");
                }
                
                return BaseModel.Success();
            }
            var noJobMessage = "No job remaining for email notification";
            _logger.Info(noJobMessage);
            return BaseModel.Success(noJobMessage);
        }
        public async Task<BaseModel> SendResourcesEmailToCustomers(List<Data.Entities.Resource> resources)
        {
            if (resources.Any())
            {


                var users = await _customerRepository.Get().Include(a => a.CoursewiseUser).Where(a => a.CoursewiseUser != null && a.CoursewiseUser.JobNotification).ToListAsync();

                if (_wordpressOptions.IsSendEmail)
                {
                    if (_hostingEnvironment.EnvironmentName != "Production")
                    {
                        var onlyTheseEmails = new List<string>() { "@wearenova.co.uk", "@seventechnology.co.uk", "@fusiontech.global", "hassankhan87@hotmail.com" };
                        users = users.Where(e => onlyTheseEmails.Exists(x => e.CoursewiseUser.Email.ToLower().Contains(x))).ToList();
                    }
                    // send notification
                    foreach (var resource in resources)
                    {
                        await _emailTemplateService.NewResource(users, resource);
                    }
                    _logger.Info($"{resources.Count} resources email notifications have been sent");
                }

                return BaseModel.Success();
            }
            var noJobMessage = "No resources remaining for email notification";
            _logger.Info(noJobMessage);
            return BaseModel.Success(noJobMessage);
        }
       
    }
}
