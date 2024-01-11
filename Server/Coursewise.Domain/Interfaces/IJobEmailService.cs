using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IJobEmailService : IGenericService<Job, Guid>
    {
        Task<BaseModel> SendEmailToCustomers();
        Task<BaseModel> SendEmailToCustomers(Guid jobId);
        Task<BaseModel> SendJobEmailToCustomers(List<Data.Entities.Job> jobs);
    }
}
