using Coursewise.Test.Mock;
using Coursewise.Test.Mock.Services;
using GenFu;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.UnitTests
{
    internal class ResourceServiceUnitTest
    {
        private readonly IResourceServiceMock _resourceServiceMock;

        public ResourceServiceUnitTest()
        {
            _resourceServiceMock = GetSetupMocks.Instance.ResourceServiceMock;
        }

        [Test]
        public async Task ResourceService_Add_Successfull()
        {
            /// Arrange
            var model = A.New<Domain.Models.Dto.AddResourceDto>();
            model.IsFeatured = false;
            model.Media = _resourceServiceMock.GetFormFile(2);
            _resourceServiceMock.GetUploadArtifact(true);
            var service = _resourceServiceMock.GetService();
            // Act
            var result = await service.Add(model);

            // Assert
            Assert.That(result.success, Is.True);
        }

        //[Test]
        //public async Task ResourceService_Add_Update_Url_If_Exist_Successfull()
        //{
        //    /// Arrange
        //    var model = A.New<Domain.Models.Dto.AddResourceDto>();
        //    model.Media = _resourceServiceMock.GetFormFile(2);
        //    _resourceServiceMock.GetUploadArtifact(true);
        //    _resourceServiceMock.GetAlreadyExistUrl();
        //    _resourceServiceMock.GetResourceById(Guid.NewGuid());
        //    var service = _resourceServiceMock.GetService();
        //    // Act
        //    var result = await service.Add(model);

        //    // Assert
        //    Assert.That(result.success, Is.True);
        //}

        [Test]
        public async Task ResourceService_Add_File_Upload_Issue_Operation_Shoulb_Be_UnSuccessfull()
        {
            /// Arrange
            var model = A.New<Domain.Models.Dto.AddResourceDto>();
            model.Media = _resourceServiceMock.GetFormFile(2);
            _resourceServiceMock.GetUploadArtifact(false);
            var service = _resourceServiceMock.GetService();
            // Act
            var result = await service.Add(model);

            // Assert
            Assert.That(result.success, Is.False);
        }
        [Test]
        public async Task ResourceService_Edit_Should_Be_Successfull()
        {
            /// Arrange
            var model = A.New<Domain.Models.Dto.EditResourceDto>();
            model.IsFeatured = false;
            _resourceServiceMock.GetExistingResource(true);
            var service = _resourceServiceMock.GetService();
            // Act
            var result = await service.Update(model);

            // Assert
            Assert.That(result.success, Is.True);
        }
        [Test]
        public async Task ResourceService_Edit_Not_Exist_Should_Be_UnSuccessfull()
        {
            /// Arrange
            var model = A.New<Domain.Models.Dto.EditResourceDto>();
            _resourceServiceMock.GetExistingResource(false);
            var service = _resourceServiceMock.GetService();
            // Act
            var result = await service.Update(model);

            // Assert
            Assert.That(result.success, Is.False);
        }
    }
}
