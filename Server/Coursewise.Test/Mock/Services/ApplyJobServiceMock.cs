using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Services;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using Coursewise.Test.Models;
using GenFu;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock.Services
{
    public interface IApplyJobServiceMock
    {
        void SetupApplyJobUnpaidMember(bool isUnpaid);
        void SetupApplyJobJobNotExist(bool isExist);
        void SetupApplyJobAlreadyApplied(Guid jobId, Guid customerId, bool isAlreadyApplied);
        void SetupApplyJobArtifactUpload(bool isToUpload, IFormFile file);
        IFormFile GetFormFile();
        void SetupApplyJobEmailsSuccessfull(bool isEmailSent);
        ApplyJobService GetService();
    }
    public class ApplyJobServiceMock: IApplyJobServiceMock
    {
        private readonly IAllMocks _allMocks;
        private readonly Mock<IWordpressService> _wordpressServiceMock;
        private readonly Mock<IArtifactService> _artifactServiceMock;
        private readonly Mock<IJobService> _jobServiceMock;
        private readonly Mock<IEmailTemplateService> _emailTemplateServiceMock;
        private readonly Mock<IApplyJobRepository> _applyJobRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<ICoursewiseLogger<ApplyJobService>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUtilityFacade> _utilityFacadeMock;
        private readonly Mock<IApplyJobServiceFacade> _applyJobServiceFacade;

        public ApplyJobServiceMock()
        {
            _allMocks = AllMocksGetter.AllMocks;
            _wordpressServiceMock = _allMocks.MockWordpressService;
            _artifactServiceMock = _allMocks.MockArtifactService;
            _jobServiceMock = _allMocks.MockJobService;
            _emailTemplateServiceMock = _allMocks.MockEmailTemplateService;
            _applyJobRepositoryMock = _allMocks.MockApplyJobRepository;
            _mapper = MockObjects.GetMapper(_allMocks.MockMapper.Object);
            _loggerMock = _allMocks.MockApplyJobServiceLogger;
            _unitOfWorkMock = _allMocks.MockUnitOfWork;
            _utilityFacadeMock = _allMocks.MockUtilityFacade;
            _applyJobServiceFacade = _allMocks.MockApplyJobServiceFacade;
        }
        public void SetupApplyJobUnpaidMember(bool isUnpaid)
        {
            _wordpressServiceMock.Setup(e => e.IsPaidUser(It.IsAny<string>())).Returns(
                async () => {
                    if (isUnpaid)
                    {
                        return await Task.FromResult(BaseModel.Error(""));
                    }
                    else
                    {
                        return await Task.FromResult(BaseModel.Success(""));
                    }
                   }
                );
           
        }
        public void SetupApplyJobAlreadyApplied(Guid jobId, Guid customerId,bool isAlreadyApplied)
        {
            var appliedJobs = A.ListOf<Data.Entities.CustomerJob>();
            _applyJobRepositoryMock.Setup(e => e.Get()).Returns(
            () => {
                if (isAlreadyApplied)
                {
                    var mock = appliedJobs.BuildMock();
                    return mock;
                }
                else
                {
                    var first = appliedJobs.FirstOrDefault();
                    first!.JobId = jobId;
                    first.CustomerId = customerId;
                    var mock = appliedJobs.BuildMock();
                    return mock;
                }
            }
            );
            
        }
        public void SetupApplyJobJobNotExist(bool isExist)
        {
            _jobServiceMock.Setup(e => e.IsJobExist(It.IsAny<Guid>())).Returns(
                async () => {
                    if (isExist)
                    {
                        return await Task.FromResult(true);
                    }
                    else
                    {
                        return await Task.FromResult(false);
                    }
                }
                );
            
        }
        public void SetupApplyJobArtifactUpload(bool isToUpload, IFormFile file)
        {
            _artifactServiceMock.Setup(e => e.UploadArtifact(file,It.IsAny<string>(), It.IsAny<bool>())).Returns(
                async () => {
                    if (isToUpload)
                    {
                        return await Task.FromResult(BaseModel.Success(data:A.New<Domain.Entities.Artifact>()));
                    }
                    else
                    {
                        return await Task.FromResult(BaseModel.Error(""));
                    }
                }
                );
        }
        public void SetupApplyJobEmailsSuccessfull(bool isEmailSent)
        {
            _emailTemplateServiceMock.Setup(e => e.ApplyJob(It.IsAny<string>(), It.IsAny<string>())).Returns(
                async () => {
                    if (isEmailSent)
                    {
                        return await Task.FromResult(BaseModel.Success());
                    }
                    else
                    {
                        return await Task.FromResult(BaseModel.Error(""));
                    }
                }
                );
            _emailTemplateServiceMock.Setup(e => e.ApplyJobAdmin(It.IsAny<Domain.Entities.CustomerJob>(), It.IsAny<IFormFile>(), It.IsAny< Domain.Entities.Job>())).Returns(
                async () => {
                    if (isEmailSent)
                    {
                        return await Task.FromResult(BaseModel.Success());
                    }
                    else
                    {
                        return await Task.FromResult(BaseModel.Error(""));
                    }
                }
                );
            _jobServiceMock.Setup(e => e.GetJobSpecifDetail(It.IsAny<Guid>())).Returns(
                async () => {
                    if (isEmailSent)
                    {
                        return await Task.FromResult(new Domain.Entities.Job() { Company = "Test"});
                    }
                    else
                    {
                        return await Task.FromResult(new Domain.Entities.Job());
                    }
                }
                );
        }
        public IFormFile GetFormFile()
        {
            var file = new Mock<IFormFile>();
            return file.Object;
        }
        public ApplyJobService GetService()
        {
            return GetApplyJobService();
        }

        private ApplyJobService GetApplyJobService()
        {
            _utilityFacadeMock.Setup(s => s.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _utilityFacadeMock.Setup(s => s.Mapper).Returns(_mapper);
            _applyJobServiceFacade.Setup(s => s.JobService).Returns(_jobServiceMock.Object);
            _applyJobServiceFacade.Setup(s => s.ApplyJobRepository).Returns(_applyJobRepositoryMock.Object);
            _applyJobServiceFacade.Setup(s => s.ArtifactService).Returns(_artifactServiceMock.Object);
            _applyJobServiceFacade.Setup(s => s.WordpressService).Returns(_wordpressServiceMock.Object);
            _applyJobServiceFacade.Setup(s => s.EmailTemplateService).Returns(_emailTemplateServiceMock.Object);
            return new ApplyJobService(_applyJobServiceFacade.Object,  _utilityFacadeMock.Object, _loggerMock.Object);
        }
    }
}
