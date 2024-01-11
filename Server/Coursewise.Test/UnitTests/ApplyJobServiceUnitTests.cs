using Coursewise.Test.Mock;
using Coursewise.Test.Mock.Services;
using GenFu;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.UnitTests
{
    internal class ApplyJobServiceUnitTests
    {
        private readonly IApplyJobServiceMock _applyJobServiceMock;
        
        public ApplyJobServiceUnitTests()
        {
            _applyJobServiceMock = GetSetupMocks.Instance.ApplyJobServiceMock;            
        }

        [Test]
        public async Task ApplyJobService_Apply_Successfull()
        {
            /// Arrange
            var jobId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var model = A.New<Domain.Models.Dto.ApplyJobDto>();
            model.JobId = jobId;
            model.File = _applyJobServiceMock.GetFormFile();

            _applyJobServiceMock.SetupApplyJobUnpaidMember(false);
            _applyJobServiceMock.SetupApplyJobJobNotExist(true);
            _applyJobServiceMock.SetupApplyJobAlreadyApplied(jobId, customerId, true);
            _applyJobServiceMock.SetupApplyJobArtifactUpload(true,model.File);
            _applyJobServiceMock.SetupApplyJobEmailsSuccessfull(true);
            var service = _applyJobServiceMock.GetService();
            // Act
            var result = await service.Apply(model, "", customerId);

            // Assert
            Assert.That(result.success, Is.True);
        }
        //[Test]
        //public async Task ApplyJobService_Apply_Successfull()
        //{
        //    /// Arrange
        //    var jobId = Guid.NewGuid();
        //    var customerId = Guid.NewGuid();
        //    var model = A.New<Domain.Models.Dto.ApplyJobDto>();
        //    model.JobId = jobId;
        //    model.File = _applyJobServiceMock.GetFormFile();

        //    _applyJobServiceMock.SetupApplyJobUnpaidMember(false);
        //    _applyJobServiceMock.SetupApplyJobJobNotExist(true);
        //    _applyJobServiceMock.SetupApplyJobAlreadyApplied(jobId, customerId, true);
        //    _applyJobServiceMock.SetupApplyJobArtifactUpload(true, model.File);
        //    _applyJobServiceMock.SetupApplyJobEmailsSuccessfull(true);
        //    var service = _applyJobServiceMock.GetService();
        //    // Act
        //    var result = await service.Apply(model, "", customerId);

        //    // Assert
        //    Assert.That(result.success, Is.True);
        //}
        [Test]
        public async Task ApplyJobService_Apply_By_UnPaid_Member_Should_Be_UnSuccessfull()
        {
            // Arrange
            _applyJobServiceMock.SetupApplyJobUnpaidMember(true);
            var service = _applyJobServiceMock.GetService();
            // Act
            var result = await service.Apply(new Domain.Models.Dto.ApplyJobDto { }, "", Guid.Empty);
            // Assert
            Assert.That(result.success, Is.False);
        }

      
        [Test]
        public async Task ApplyJobService_Apply_On_Already_Applied_Job_Should_Be_UnSuccessfull()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            _applyJobServiceMock.SetupApplyJobUnpaidMember(false);
            _applyJobServiceMock.SetupApplyJobJobNotExist(true);
            _applyJobServiceMock.SetupApplyJobAlreadyApplied(jobId, customerId,false);
            var service = _applyJobServiceMock.GetService();
            // Act
            var result = await service.Apply(new Domain.Models.Dto.ApplyJobDto { JobId=jobId }, "", customerId);
            
            // Assert
            Assert.That(result.success, Is.False);
        }
    }
}
