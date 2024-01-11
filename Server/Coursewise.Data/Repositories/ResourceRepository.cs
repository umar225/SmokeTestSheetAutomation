using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Repositories
{
    public class ResourceRepository : GenericRepository<Resource, Guid>, IResourceRepository
    {
        private readonly DbSet<ResourceArtifact> _resourceArtifactDbContext;

        public ResourceRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
            _resourceArtifactDbContext = dbContext.ResourceArtifacts;   
        }

        public async Task<ResourceArtifact> AddArtifact(ResourceArtifact resourceArtifact)
        {
            var entity=await _resourceArtifactDbContext.AddAsync(resourceArtifact);
            return entity.Entity;
        }

        public async Task<ResourceArtifact?> GetArtifact(int resourceArtifactId)
        {
            var entity = await _resourceArtifactDbContext.AsQueryable()
                .Include(i=>i.Variations)
                    .ThenInclude(i=> i.Artifact)
                .Include(i=>i.Artifact)
                .FirstOrDefaultAsync(a=>a.Id == resourceArtifactId);
            return entity;
        }
        public async Task<int> NoOfOrignalArtifacts(Guid resourceId)
        {
            var count = await _resourceArtifactDbContext.AsQueryable()                
                .CountAsync(a => a.ResourceId == resourceId && a.ParentResourceArtifactId==null);
            return count;
        }
        public async Task DeleteArtifact(ResourceArtifact resourceArtifact)
        {
            await Task.Run(() => _resourceArtifactDbContext.Remove(resourceArtifact));            
        }
    }
}
