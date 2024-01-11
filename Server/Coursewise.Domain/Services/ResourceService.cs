using AutoMapper;
using Coursewise.AWS.Communication;
using Coursewise.Common.Exceptions;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Extensions;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Models.Dto;
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
    public class ResourceService : GenericService<Data.Entities.Resource, Resource, Guid>, IResourceService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IArtifactService _artifactService;
        private readonly ICustomerService _customerService;
        private readonly ICoursewiseLogger<ResourceService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResourceService(
            IResourceServiceFacade resourceServiceFacade,
            ICoursewiseLogger<ResourceService> logger,
            IUtilityFacade utilityFacade) : base(utilityFacade.Mapper, resourceServiceFacade.ResourceRepository, utilityFacade.UnitOfWork)
        {
            _resourceRepository = resourceServiceFacade.ResourceRepository;
            _artifactService = resourceServiceFacade.ArtifactService;
            _customerService = resourceServiceFacade._customerService;
            _logger = logger;
            _mapper = utilityFacade.Mapper;
            _unitOfWork = utilityFacade.UnitOfWork;
        }

        public async Task<BaseModel> Add(Models.Dto.AddResourceDto businessEntity)
        {
            List<string> fileNames = new List<string>();
            List<ResourceArtifact> artifacts = new List<ResourceArtifact>();
            string errorMessage = "Some issue in adding resource.";            
            try
            {
                var resource = _mapper.Map<Resource>(businessEntity);
                var url = Common.Utilities.StringHelper.UrlFriendly(resource.Title);
                resource.Url = url;
                var existingResourceWithSameUrl = await _resourceRepository.FirstOrDefaultAsync(c => c.Url == url);
                foreach (var file in businessEntity.Media)
                {
                    var uploadResponse = await _artifactService.UploadArtifact(file, "resource");
                    if (!uploadResponse.success)
                    {
                        throw new AwsFileUploadException();
                    }
                    var artifact = _mapper.Map<Artifact>(uploadResponse.data);
                    fileNames.Add(artifact.StorageLocation);
                    var orignal = new ResourceArtifact { Artifact = artifact, Type = "Orignal" };
                    artifacts.Add(orignal);
                }
                resource.ResourceArtifacts = artifacts;
                resource.CreatedAt = DateTime.Now.ToUniversalTime();
                resource.NoOfClaps = 0;
                resource.IsReady = true;
                
                
                var resourceDataEntity = _mapper.Map<Data.Entities.Resource>(resource);
                resourceDataEntity.IsEmailSent = !businessEntity.SendNotification;
                await _resourceRepository.AddAsync(resourceDataEntity);
                await _unitOfWork.SaveChangesAsync();
                if (existingResourceWithSameUrl != null)
                {
                    url = url + "-" + resourceDataEntity.Id.ToString();
                    var getResource = await _resourceRepository.GetAsync(resourceDataEntity.Id);
                    getResource.Url = url;
                    await _resourceRepository.UpdateAsync(getResource);
                    await _unitOfWork.SaveChangesAsync();
                }
                return BaseModel.Success(resourceDataEntity.Id);
            }
            catch (AwsFileUploadException)
            {
                _logger.Info($"As resource operation not completed, so deleting its files, {string.Join(",", fileNames)}");
                await _artifactService.DeleteArtifacts(fileNames);
                return BaseModel.Error(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                _logger.Info($"As resource operation not completed, so deleting its files, {string.Join(",", fileNames)}");
                await _artifactService.DeleteArtifacts(fileNames);
                return BaseModel.Error(errorMessage);
            }
        }
        public async Task ChangeFeatured(bool isFeature,Guid? notThisOne=null)
        {
            if (isFeature)
            {
                System.Linq.Expressions.Expression<Func<Data.Entities.Resource, bool>> where = (notThisOne is not null && notThisOne != Guid.Empty) ? (x => x.IsFeatured && x.Id != notThisOne) : x => x.IsFeatured;
                var existingFeaturedResources = await _resourceRepository.Get().Where(where).ToListAsync();
                if (existingFeaturedResources.Any())
                {
                    existingFeaturedResources.ForEach(r => r.IsFeatured = false);
                    await _resourceRepository.UpdateAsync(existingFeaturedResources);
                }
            }
        }
        public async Task<BaseModel> GetAll()
        {
            var resources = await _resourceRepository.Get()
                                .Include(i => i.ResourceArtifacts)
                                .ThenInclude(i => i!.Artifact)
                                .Where(a=>!a.isDeleted)
                                .Select(s => new ResourceBasicDto
                                {
                                    Id = s.Id,
                                    Url = s.Url,
                                    Title = s.Title,
                                    Length = s.Length,
                                    Preview = s.Preview,
                                    IsFeatured = s.IsFeatured,
                                    NoOfClaps = s.NoOfClaps,
                                    Time = s.CreatedAt,
                                    Media = (s.ResourceArtifacts.Any() && s.ResourceArtifacts.FirstOrDefault()!.Artifact != null) ? s.ResourceArtifacts.FirstOrDefault()!.Artifact.Url : ""
                                }).ToListAsync();
            return BaseModel.Success(resources);
        }
        public async Task<IList<ResourceDetailDto>> GetLatest()
        {
            return await GetResourcesDetailsAsync().OrderBy(a=>a.Time).Take(2).ToListAsync();
        }
        public async Task<BaseModel> Update(Models.Dto.EditResourceDto model)
        {
            var existingResource = await _resourceRepository.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (existingResource == null)
            {
                return BaseModel.Error("No resource exist");
            }
            var url = Common.Utilities.StringHelper.UrlFriendly(model.Title);
            var existingResourceWithSameUrl = await _resourceRepository.FirstOrDefaultAsync(c =>c.Id != existingResource.Id && c.Url == url);
            if (existingResourceWithSameUrl != null)
            {
                url = url + "-" + existingResource.Id.ToString();
            }
            existingResource.Preview = model.Preview;
            existingResource.Title = model.Title;
            existingResource.Description = model.Description;
            existingResource.Length = model.Length;
            existingResource.Url = url;
            existingResource.IsReady = true;
            existingResource.IsFeatured = model.IsFeatured;
            existingResource.ResourceType = model.ResourceType;
            existingResource.VideoLink = model.VideoLink;
            existingResource.CreatedAt = DateTime.Now.ToUniversalTime();
            await _resourceRepository.UpdateAsync(existingResource);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success(existingResource.Id);

        }
        public new  async Task<BaseModel> Get(Guid id)
        {
            var resource = await GetResourcesDetailsAsync().FirstOrDefaultAsync(f=> f.Id==id);
            if (resource==null)
            {
                return BaseModel.Error("No resource exist");
            }
            return BaseModel.Success(resource);
        }
        private  IQueryable<ResourceDetailDto> GetResourcesDetailsAsync()
        {
            return  _resourceRepository.Get()
                                .Include(i => i.ResourceArtifacts.Where(i => i.Type == "Orignal"))
                                    .ThenInclude(i => i.Artifact)
                                .Select(s => new Models.Dto.ResourceDetailDto
                                {
                                    Id = s.Id,
                                    Description = s.Description.Substring(0,150),
                                    Title = s.Title,
                                    Length = s.Length,
                                    Preview = s.Preview,
                                    IsFeatured = s.IsFeatured,
                                    Time = s.CreatedAt,
                                    NoOfClaps = s.NoOfClaps,
                                    Url=s.Url,
                                    ResourceType=s.ResourceType,
                                    VideoLink=s.VideoLink,
                                    Media = s.ResourceArtifacts.Select(s => new Models.Dto.ResourceMediaDto
                                    {
                                        Id = s.Id,
                                        Url = s.Artifact.Url,
                                        ArtifactType = s.Artifact.FileType
                                    })
                                });
                                                                      
        }
        public async Task<BaseModel> UploadMedia(Models.Dto.ResourceMediaUploadDto media)
        {
            var resource = await _resourceRepository.GetAsync(media.ResourceId);
            if (resource is null)
            {
                return BaseModel.Error("Resource dose not exist");
            }
            var uploadResponse = await _artifactService.UploadArtifact(media.File, "resource");
            if (!uploadResponse.success)
            {
                BaseModel.Error("Some issue in uploading this file");
            }
            var artifact = _mapper.Map<Artifact>(uploadResponse.data);
            var dataArtifact = _mapper.Map<Data.Entities.Artifact>(artifact);
            var orignal = new Data.Entities.ResourceArtifact { Artifact = dataArtifact, Type = "Orignal", ResourceId=resource.Id };
            var resourceArtifact = await _resourceRepository.AddArtifact(orignal);
            await _unitOfWork.SaveChangesAsync();
            var mediaEntity = new Models.Dto.ResourceMediaDto
            {
                Id = resourceArtifact.Id,
                Url = resourceArtifact.Artifact.Url,
                ArtifactType = resourceArtifact.Artifact.FileType
            };
            return BaseModel.Success(mediaEntity);
        }
        public async Task<BaseModel> DeleteMedia(int resourceArtifactId)
        {
            var resourceArtifact = await _resourceRepository.GetArtifact(resourceArtifactId);
            if (resourceArtifact is null)
            {
                return BaseModel.Error("Resource media does not exist");
            }
            foreach (var variation in resourceArtifact.Variations)
            {
                await _artifactService.DeleteArtifact(variation.Artifact.StorageLocation);
            }
            await _artifactService.DeleteArtifact(resourceArtifact.Artifact.StorageLocation);
            await _resourceRepository.DeleteArtifact(resourceArtifact);
            await _artifactService.DeleteArtifactEntry(resourceArtifact.Artifact);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }
        public async Task<BaseModel> DeleteResource(Guid resourceId)
        {

            var resource = await _resourceRepository.FirstOrDefaultAsync(r => r.Id == resourceId);
            if (resource == null)
            {
                return BaseModel.Error("Resource does not exist");
            }
            resource.isDeleted= true;
            await _resourceRepository.UpdateAsync(resource);
            await _unitOfWork.SaveChangesAsync();
          
            return BaseModel.Success();
        }
        private async Task<List<Models.Dto.ResourceDetailDto>> Filters(Models.ResourceFilters filters,bool isPaidUser)
        {
            var currentDate = DateTime.Now.ToUniversalTime();
            filters.Next = (filters.Next is null) ? currentDate : filters.Next;
            if (filters.SearchString == "null")
            {
                filters.SearchString = null;
            }
            var resoursesQuery = _resourceRepository.Get();
            if (!isPaidUser)
            {
                resoursesQuery = resoursesQuery.Where(a => a.ResourceType != (int)ResourceType.VIDEO_RESOURCE);
            }
            if (string.IsNullOrWhiteSpace(filters.SearchString))
            {
                resoursesQuery = resoursesQuery.Where(j => j.CreatedAt < filters.Next && j.IsReady && !j.isDeleted);
                               
            }
            else
            {
                resoursesQuery = resoursesQuery.Where(j => j.CreatedAt < filters.Next && j.IsReady && !j.isDeleted
                && (j.Title.Contains(filters.SearchString) || j.Preview.Contains(filters.SearchString) || j.Description.Contains(filters.SearchString)));
                               
            }
            resoursesQuery=resoursesQuery.Include(i => i.ResourceArtifacts.Where(i => i.Type == "Orignal"))
                                .ThenInclude(i => i.Artifact)
                                .OrderByDescending(i => i.IsFeatured)
                                .ThenByDescending(i => i.CreatedAt)
                                    .AsQueryable();



            var resourses = await resoursesQuery.Select(s => new Models.Dto.ResourceDetailDto
                            {
                                Id = s.Id,
                                Description =isPaidUser?s.Description: s.Description.Substring(0,150),
                                Title = s.Title,
                                Length = s.Length,
                                Preview = s.Preview,
                                IsFeatured = s.IsFeatured,
                                Time = s.CreatedAt,
                                Url = s.Url,
                                NoOfClaps = s.NoOfClaps,
                                ResourceType = s.ResourceType,
                                VideoLink =  isPaidUser ? s.VideoLink : "",
                                Media = s.ResourceArtifacts.Select(s => new Models.Dto.ResourceMediaDto
                                {
                                    Id = s.Id,
                                    Url = s.Artifact.Url,
                                    ArtifactType = s.Artifact.FileType
                                })
                            }).Take(6).ToListAsync();
            
            return resourses;
        }
        public async Task<BaseModel> GetByFilters(Models.ResourceFilters filters,string? userId)
        {
            var isPaidUser = await _customerService.isSubscribed(userId);
            var resources = await Filters(filters, isPaidUser);
            var lastResource = resources.LastOrDefault();
            DateTime? dateTime = null;
            dateTime = (lastResource is null) ? dateTime : lastResource.Time;
            return BaseModel.Success(new { next=dateTime, resources });
        }

        public async Task<BaseModel> GetByUrl(string url,string? userId)
        {
            var isPaidUser =  await _customerService.isSubscribed(userId);
            dynamic user= new System.Dynamic.ExpandoObject();
            user.isPaidUser = isPaidUser;
            var isFreeResources = true;
            if (!isPaidUser)
            {
                isFreeResources= await _customerService.isFreeResourcesAvailble(userId);
               
            }
           
            var resource = await _resourceRepository.Get()
                .Where(s => s.Url == url && s.IsReady)
                .Select(s => new Models.Dto.ResourceDetailDto
                {
                    Id = s.Id,
                    Description =isFreeResources? s.Description:s.Description.Substring(0,500),
                    Title = s.Title,
                    Length = s.Length,
                    Preview = s.Preview,
                    IsFeatured = s.IsFeatured,
                    Time = s.CreatedAt,
                    IsReady = s.IsReady,
                    NoOfClaps = s.NoOfClaps,
                    ResourceType = s.ResourceType,
                    VideoLink =isPaidUser? s.VideoLink:"",
                    Media = s.ResourceArtifacts.Select(s => new Models.Dto.ResourceMediaDto
                    {
                        Id = s.Id,
                        Url = s.Artifact.Url,
                        ArtifactType = s.Artifact.FileType
                    })
                }).FirstOrDefaultAsync();
            if (resource == null)
            {
                return BaseModel.Error("This resource does not exist");
            }
            if (isFreeResources&&resource.ResourceType==(int)ResourceType.IMAGE_RESOURCE)
            {
                user.FreeResourcesAvailable = await _customerService.UpdateFreeResources(userId);
            }
            else
            {
                user.FreeResourcesAvailable = 0;

            }
            return BaseModel.Success(new { user,resource });
        }
        public async Task<BaseModel> GetFeatured()
        {
            var resoursesQuery = _resourceRepository.Get().Where(j => j.IsReady && j.IsFeatured)
                                .Include(i => i.ResourceArtifacts.Where(i => i.Type == "Orignal"))
                                    .ThenInclude(i => i.Artifact);

            var resourse = await resoursesQuery.Select(s => new Models.Dto.ResourceDetailDto
            {
                Id = s.Id,
                Description = s.Description,
                Title = s.Title,
                Length = s.Length,
                Preview = s.Preview,
                IsFeatured = s.IsFeatured,
                Time = s.CreatedAt,
                Url = s.Url,
                NoOfClaps = s.NoOfClaps,
                ResourceType = s.ResourceType,
                VideoLink = s.VideoLink,
                Media = s.ResourceArtifacts.Select(s => new Models.Dto.ResourceMediaDto
                {
                    Id = s.Id,
                    Url = s.Artifact.Url,
                    ArtifactType = s.Artifact.FileType
                })
            }).FirstOrDefaultAsync();
            if (resourse == null)
            {
                return BaseModel.Success("No featured article available at the movement");
            }
            return BaseModel.Success(resourse);
        }

        public async Task<BaseModel> Applause(Guid customerId, Guid id,string? userId)
        {
            var isPaidUser =  await _customerService.isSubscribed(userId);
            if (!isPaidUser)
            {
                return BaseModel.Error("");
            }
            var resource = await _resourceRepository.FirstOrDefaultAsync(r=>r.Id == id && r.IsReady);
            if (resource == null)
            {
                return BaseModel.Error("Resource does not exist");
            }
            resource.NoOfClaps += 1;
            await _resourceRepository.UpdateAsync(resource);
            await _unitOfWork.SaveChangesAsync();
            _logger.Info($"User {customerId} has applauded against this resource {id} - {resource.Title} - applaud count {resource.NoOfClaps}");
            return BaseModel.Success(resource.NoOfClaps);
        }

    }
}
