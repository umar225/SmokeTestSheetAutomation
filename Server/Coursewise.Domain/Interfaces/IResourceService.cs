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
    public interface IResourceService : IGenericService<Resource, Guid>
    {
        Task<BaseModel> Add(Models.Dto.AddResourceDto businessEntity);
        Task<BaseModel> GetAll();
        Task<BaseModel> Update(Models.Dto.EditResourceDto model);
        new Task<BaseModel> Get(Guid id);
        Task<BaseModel> UploadMedia(Models.Dto.ResourceMediaUploadDto media);
        Task<BaseModel> DeleteMedia(int resourceArtifactId);
        Task<BaseModel> GetByFilters(Models.ResourceFilters filters,string? userId);
        Task<BaseModel> GetByUrl(string url,string? userId);
        Task<BaseModel> GetFeatured();
        Task<BaseModel> Applause(Guid customerId, Guid id,string? userId);
        Task<IList<ResourceDetailDto>> GetLatest();
        Task<BaseModel> DeleteResource(Guid resourceId);
    }
}
