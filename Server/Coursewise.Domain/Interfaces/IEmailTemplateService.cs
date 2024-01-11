using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IEmailTemplateService : IGenericService<EmailTemplate, int>
    {
        Task<EmailTemplate> GetByType(EmailType emailType);
        Task<BaseModel> ApplyJob(string toEmail, string name);
        Task<BaseModel> ApplyJobAdmin(CustomerJob customerJob, IFormFile cv, Job job);
        Task<BaseModel> JobOpportunity(List<Data.Entities.Customer> users, Data.Entities.Job job);
        Task<BaseModel> ResetPassword(string toEmail, string name, string resetToken, string userId);
        Task<BaseModel> ConfirmEmail(string toEmail, string name, string confirmToken, string userId);
        Task<BaseModel> NewResource(List<Data.Entities.Customer> users, Data.Entities.Resource resource);

    }
}
