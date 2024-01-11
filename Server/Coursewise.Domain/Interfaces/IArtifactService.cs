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
    public interface IArtifactService : IGenericService<Artifact, int>
    {
        Task<BaseModel> UploadArtifact(IFormFile file, string fileLoaction, bool? isFilePermissionPrivate=null);
        string GetFileUrl(string fileName);
        Task<BaseModel> DeleteArtifacts(IEnumerable<string> fileNames);
        Task<BaseModel> DeleteArtifact(string fileName);
        Task DeleteArtifactEntry(Data.Entities.Artifact artifact);
    }
}
