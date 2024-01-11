using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Services;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using GenFu;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock.Services
{
    public interface IResourceServiceMock
    {
        void GetUploadArtifact(bool isSuccessful);
        ResourceService GetService();
        void GetAlreadyExistUrl();
        void GetResourceById(Guid id);
        List<IFormFile> GetFormFile(int noOfFiles);
        void GetExistingResource(bool isExist);
    }
    public class ResourceServiceMock: IResourceServiceMock
    {
        private readonly IAllMocks _allMocks;
        private readonly Mock<IArtifactService> _artifactServiceMock;
        private readonly Mock<IResourceRepository> _resourceRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<ICoursewiseLogger<ResourceService>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUtilityFacade> _utilityFacadeMock;
        private readonly Mock<IResourceServiceFacade> _resourceServiceFacade;

        public ResourceServiceMock()
        {
            _allMocks = AllMocksGetter.AllMocks;
            _artifactServiceMock = _allMocks.MockArtifactService;
            _resourceRepositoryMock = _allMocks.MockResourceRepository;
            _mapper = MockObjects.GetMapper(_allMocks.MockMapper.Object);
            _loggerMock = _allMocks.MockResourceServiceLogger;
            _resourceServiceFacade = _allMocks.MockResourceServiceFacade;
            _utilityFacadeMock = _allMocks.MockUtilityFacade;
            _unitOfWorkMock = _allMocks.MockUnitOfWork;
        }

        public void GetUploadArtifact(bool isSuccessful)
        {
            _artifactServiceMock.Setup(s => s.UploadArtifact(It.IsAny<IFormFile>(), It.IsAny<string>(), default)).ReturnsAsync(
                () =>
                {
                    if (isSuccessful)
                    {
                        return BaseModel.Success(data:new Domain.Entities.Artifact());
                    }
                    return BaseModel.Error("");
                });
        }
        public void GetAlreadyExistUrl()
        {
            _resourceRepositoryMock.Setup(s => s.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Data.Entities.Resource,bool>>>(),"")).ReturnsAsync(
                () =>
                {
                    var resource= A.New<Data.Entities.Resource>();
                    return resource;
                });
        }
        public void GetResourceById(Guid id)
        {
            _resourceRepositoryMock.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(
                () =>
                {
                    var resource = A.New<Data.Entities.Resource>();
                    resource.Id = id;
                    return resource;
                });
        }
        public List<IFormFile> GetFormFile(int noOfFiles)
        {
            var files = new List<IFormFile>();
            for (int i= 0; i<noOfFiles;i++)
            {
                files.Add((new Mock<IFormFile>()).Object);
            }
            return files;
        }
        public void GetExistingResource(bool isExist)
        {
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            _resourceRepositoryMock.Setup(s => s.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Data.Entities.Resource, bool>>>(), "")).ReturnsAsync(
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                () =>
                {
                    if (isExist)
                    {
                        var resource = A.New<Data.Entities.Resource>();
                        return resource;
                    }
                    else
                    {
                        return null;
                    }
                    
                });
        }

        public ResourceService GetService()
        {
            _resourceServiceFacade.Setup(s=> s.ResourceRepository).Returns(_resourceRepositoryMock.Object);
            _resourceServiceFacade.Setup(s => s.ArtifactService).Returns(_artifactServiceMock.Object);
            _utilityFacadeMock.Setup(s=>s.Mapper).Returns(_mapper);
            _utilityFacadeMock.Setup(s => s.UnitOfWork).Returns(_unitOfWorkMock.Object);
            return new ResourceService(_resourceServiceFacade.Object,_loggerMock.Object,_utilityFacadeMock.Object);
        }
    }
}
