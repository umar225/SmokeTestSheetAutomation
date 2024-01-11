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
    public class JobRepository:GenericRepository<Job,Guid>, IJobRepository
    {
        private readonly DbSet<Job> _jobDbContext;

        public JobRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
            _jobDbContext = dbContext.Jobs;
        }

        public async Task<bool> IsJobExist(Guid jobId)
        {
            var isExist =await _jobDbContext.AsQueryable().AnyAsync(j => j.Id == jobId && !j.IsClosed);
            return isExist;
        }
    }
}
