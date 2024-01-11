using AutoMapper;
using Coursewise.AWS.Communication;
using Coursewise.AWS.Communication.Models;
using Coursewise.Common.Models;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class EmailTemplateService : GenericService<Data.Entities.EmailTemplate, EmailTemplate, int>, IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailtemplateRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ICoursewiseLogger<EmailTemplateService> _logger;
        private readonly Models.AppOptions _options;
        public EmailTemplateService(
            ICoursewiseLogger<EmailTemplateService> logger,
            IEmailService emailService,
            IEmailTemplateRepository emailtemplateRepository,
            
            IOptionsSnapshot<Models.AppOptions> options,
            IUtilityFacade utilityFacade) : base(utilityFacade.Mapper, emailtemplateRepository, utilityFacade.UnitOfWork)
        {
            _mapper = utilityFacade.Mapper;
            _logger = logger;
            _emailtemplateRepository = emailtemplateRepository;
            _emailService = emailService;
            _options = options.Value;            
        }

        public async Task<EmailTemplate> GetByType(EmailType emailType)
        {
            var template = await _emailtemplateRepository.GetAsync(x => (int)x.Type == (int)emailType);
            var businessEntity = _mapper.Map<EmailTemplate>(template.FirstOrDefault());
            return businessEntity;
        }

        public async Task<BaseModel> ApplyJob(string toEmail, string name)
        {
            var errorMessage = "Some issue in sending a apply job email to you";
            var templ = await GetByType(EmailType.APPLY_JOB);
            if (templ == null)
            {
                _logger.Error("Apply job email template does not exist");
                return BaseModel.Error(errorMessage);
            }

            templ.Body = templ.Body.Replace("@Username", name);
            templ.Body = templ.Body.Replace("@Wplink", _options.BoardwiseWPUrl);
            var isSend = await _emailService.SendEmail(new Email
            {
                To = toEmail,
                FromEmail = _options.Emails.ApplyJob,
                Subject = "Confirming your application on Boardwise",
                Content = templ.Body
            });
            if (isSend)
            {
                return BaseModel.Success();
            }
            else
            {
                return BaseModel.Error(errorMessage);
            }
        }
        public async Task<BaseModel> ResetPassword(string toEmail, string name,string resetToken,string userId)
        {
            var errorMessage = "Some issue in sending a reset password email to you";
            var templ = await GetByType(EmailType.RESET_PASSWORD);
            if (templ == null)
            {
                _logger.Error("Reset password email template does not exist");
                return BaseModel.Error(errorMessage);
            }

            templ.Body = templ.Body.Replace("@Username", name);
            templ.Body = templ.Body.Replace("@resetlink", _options.AppUrl+"/reset-password/?userId="+userId+"&&token="+resetToken);
            var isSend = await _emailService.SendEmail(new Email
            {
                To = toEmail,
                FromEmail = _options.Emails.ApplyJob,
                Subject = "Reset Password on Boardwise",
                Content = templ.Body
            });
            if (isSend)
            {
                return BaseModel.Success();
            }
            else
            {
                return BaseModel.Error(errorMessage);
            }
        }
        public async Task<BaseModel> ConfirmEmail(string toEmail, string name, string confirmToken, string userId)
        {
            var errorMessage = "Some issue in sending a email verifcation email to you";
            var templ = await GetByType(EmailType.CONFIRM_EMAIL);
            if (templ == null)
            {
                _logger.Error("Confirm email template does not exist");
                return BaseModel.Error(errorMessage);
            }

            templ.Body = templ.Body.Replace("@Username", name);
            templ.Body = templ.Body.Replace("@confirmLink", _options.AppUrl + "/confirm-email/?userId=" + userId + "&&token=" + confirmToken);
            var isSend = await _emailService.SendEmail(new Email
            {
                To = toEmail,
                FromEmail = _options.Emails.ApplyJob,
                Subject = "Boardwise Email Verification",
                Content = templ.Body
            });
            if (isSend)
            {
                return BaseModel.Success();
            }
            else
            {
                return BaseModel.Error(errorMessage);
            }
        }
        
        public async Task<BaseModel> ApplyJobAdmin(CustomerJob customerJob,IFormFile cv, Job job)
        {
            var fileExtension = cv.FileName.Substring(cv.FileName.LastIndexOf('.'));
            var body = $"<p>Company: {job.Company}<br><br>Applicant Name: {customerJob.Name}<br><br>Email Address: {customerJob.Email}<br><br>{customerJob.Description}</p>";
            BaseModel response;
            using (var memoryStream = cv.OpenReadStream())
            {
                var attachments = new List<EmailAttachment>()
                {
                    new EmailAttachment()
                    {
                        DataStream = memoryStream,
                        Name = $"CV{fileExtension}"
                    }
                };
                var subject = $"New job application for Non-Executive Director – {job.Company}";
                if (job.JobTitles is not null && job.JobTitles.Any())
                {
                    var titles = (job.JobTitles.Count() > 1) ? string.Join(", ", job.JobTitles.Select(x => x.Title.Name)) : job.JobTitles.FirstOrDefault()!.Title.Name;
                    subject = $"New job application for {titles} – {job.Company}";
                }
                var isSend = await _emailService.SendRawEmail(new Email
                {
                    To = _options.Emails.ToApplyJobAdmin,
                    FromEmail = _options.Emails.ApplyJobAdmin,
                    Subject = subject,
                    Content = body,
                    Attachments = attachments
                });
                if (isSend)
                {
                    response = BaseModel.Success();
                }
                else
                {
                    response = BaseModel.Error("Issue in sending email template to Admin");
                }
            }
            return response;
        }

        public async Task<BaseModel> JobOpportunity(List<Data.Entities.Customer> users,Data.Entities.Job job)
        {
            var errorMessage = "Some issue in sending a job opportunity email to the customers";
            var templ = await GetByType(EmailType.JOB_OPPORTUNITY);
            if (templ == null)
            {
                _logger.Error("Job opportunity email template does not exist");
                return BaseModel.Error(errorMessage);
            }

            var titles = (job.JobTitles.Count() > 1) ? string.Join(", ", job.JobTitles.Select(x => x.Title.Name)) : job.JobTitles.FirstOrDefault()!.Title.Name;
            templ.Body = templ.Body.Replace("@JobLink", $"{_options.AppUrl}/job/{job.Id}");
            templ.Body = templ.Body.Replace("@CompanyName", job.Company);
            templ.Body = templ.Body.Replace("@JobTitle", titles);
            templ.Body = templ.Body.Replace("[NotificationsLink]", $"{_options.AppUrl}/account");


            foreach (var user in users)
            {
                string body = templ.Body;
                body = body.Replace("@Username", user.FirstName);
                if (!String.IsNullOrEmpty(user.Email))
                { 
                    await _emailService.SendEmail(new Email
                    {
                        To = user.Email,
                        FromEmail = _options.Emails.NoReply,
                        Subject = $"New Job Opportunity - {titles} - {job.Company}",
                        Content = body
                    });
                }
            }
            return BaseModel.Success();
        }
        public async Task<BaseModel> NewResource(List<Data.Entities.Customer> users, Data.Entities.Resource resource)
        {
            var errorMessage = "Some issue in sending a new resource email to the customers";
            var templ = await GetByType(EmailType.NEW_RESOURCE);
            if (templ == null)
            {
                _logger.Error("New resource email template does not exist");
                return BaseModel.Error(errorMessage);
            }

            templ.Body = templ.Body.Replace("@ResourceLink", $"{_options.AppUrl}/resource/{resource.Url}");
            templ.Body = templ.Body.Replace("@ResourceTitle", resource.Title);
            templ.Body = templ.Body.Replace("[NotificationsLink]", $"{_options.AppUrl}/account");


            foreach (var user in users)
            {
                string body = templ.Body;
                body = body.Replace("@Username", user.FirstName);
                if (!String.IsNullOrEmpty(user.Email))
                {
                    await _emailService.SendEmail(new Email
                    {
                        To = user.Email,
                        FromEmail = _options.Emails.NoReply,
                        Subject = $"New Post - {resource.Title}",
                        Content = body
                    });
                }
            }
            return BaseModel.Success();
        }
    }
}
