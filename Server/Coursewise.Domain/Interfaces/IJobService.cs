using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Models.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IJobService : IGenericService<Job, Guid>
    {
        Task<BaseModel> Add(Models.Dto.JobDto businessEntity);
        Task<BaseModel> Update(Models.Dto.EditJobDto businessEntity);
        Task<BaseModel> Delete(Guid id);
        Task<BaseModel> UploadPicture(Models.Dto.JobPictureUploadDto model);
        Task<BaseModel> GetAll(Guid customerId);
        new Task<BaseModel> Get(Guid id);
        Task<BaseModel> GetByCustomer(Guid id,Guid customerId, string? userId);
        Task<BaseModel> GetForCustomer(string? userId);
        Task<bool> IsJobExist(Guid jobId);
        Task<Job> GetJobSpecifDetail(Guid id);
        Task<BaseModel> GetJobsCounts();
        Task<BaseModel> Filter(Models.JobFilters filters, Guid customerId, string? userId);
        Task<BaseModel> CalculateJobsCounts();
        Task<BaseModel> UpdateJobsCounts();
        Task<IList<JobDetaiDto>> GetLatest(Guid customerId, string? userId);


    }
}
