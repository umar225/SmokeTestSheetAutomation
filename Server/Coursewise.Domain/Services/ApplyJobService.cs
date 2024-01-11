using AutoMapper;
using Coursewise.AWS.Communication;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class ApplyJobService : GenericService<Data.Entities.CustomerJob, CustomerJob, int>, IApplyJobService
    {
        private readonly IApplyJobRepository _applyJobRepository;
        private readonly IWordpressService _wordpressService;
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailTemplateService _emailTemplateService;

        public ApplyJobService(
            IApplyJobServiceFacade applyJobServiceFacade,
            IUtilityFacade utilityFacade,
            ICoursewiseLogger<ApplyJobService> logger
            ) : base(utilityFacade.Mapper, applyJobServiceFacade.ApplyJobRepository, utilityFacade.UnitOfWork)
        {
            _applyJobRepository = applyJobServiceFacade.ApplyJobRepository;
            _wordpressService = applyJobServiceFacade.WordpressService;
            _jobService = applyJobServiceFacade.JobService;
            _mapper = utilityFacade.Mapper;
            _emailTemplateService = applyJobServiceFacade.EmailTemplateService;
            _unitOfWork = utilityFacade.UnitOfWork;
        }

        public async Task<BaseModel> Apply(Models.Dto.ApplyJobDto model, string customerWpToken, Guid customerId)
        {
            var validResponse = await ApplyValidation(model, customerWpToken, customerId);
            if (!validResponse.success)
            {
                return validResponse;
            }
            var applyJob = _mapper.Map<CustomerJob>(model);
            
            applyJob.JobId = model.JobId;
            applyJob.CustomerId = customerId;
            applyJob.ApplyAt = DateTime.Now.ToUniversalTime();
            
            
            var applyJobDataEntity = _mapper.Map<Data.Entities.CustomerJob>(applyJob);
            await _applyJobRepository.AddAsync(applyJobDataEntity);
            await _unitOfWork.SaveChangesAsync();
            await SendApplyEmails(applyJob, model.File!);
            return BaseModel.Success(applyJobDataEntity.Id,0,"Successfully Applied");
        }
        public async Task<BaseModel> ApplyValidation(Models.Dto.ApplyJobDto model, string customerWpToken, Guid customerId)
        {
            var isPaidUser = await _wordpressService.IsPaidUser(customerWpToken);
            if (!isPaidUser.success)
            {
                return isPaidUser;
            }
            var isJobExist = await _jobService.IsJobExist(model.JobId);
            if (!isJobExist)
            {
                return BaseModel.Error("Job does not exist");
            }
            var isAlreadyApply = await _applyJobRepository.Get().AnyAsync(a => a.JobId == model.JobId && a.CustomerId == customerId);
            if (isAlreadyApply)
            {
                return BaseModel.Error("You have already applied for this job");
            }
            return BaseModel.Success();
        }
        public async Task<BaseModel> SendApplyEmails(CustomerJob customerJob,IFormFile file)
        {
            var sendResponse = await _emailTemplateService.ApplyJob(customerJob.Email, customerJob.Name);
            var job = await _jobService.GetJobSpecifDetail(customerJob.JobId);
            var sendAdminResponse = await _emailTemplateService.ApplyJobAdmin(customerJob, file, job);
            if (sendAdminResponse.success && sendResponse.success)
            {
                return BaseModel.Success();
            }
            return BaseModel.Error("Issue in sending confirmation email");
        }
    }
}
