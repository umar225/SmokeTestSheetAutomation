using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Interfaces
{
    public interface IResourceRepository : IGenericRepository<Resource, Guid>
    {
        Task<ResourceArtifact> AddArtifact(ResourceArtifact resourceArtifact);
        Task<ResourceArtifact?> GetArtifact(int resourceArtifactId);
        Task<int> NoOfOrignalArtifacts(Guid resourceId);
        Task DeleteArtifact(ResourceArtifact resourceArtifact);
    }
}
