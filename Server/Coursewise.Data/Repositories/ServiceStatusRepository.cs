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
    public class ServiceStatusRepository : GenericRepository<ServiceStatus,int>, IServiceStatusRepository
    {
        private readonly DbSet<ServiceStatus> _serviceStatusesDbContext;

        public ServiceStatusRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
            _serviceStatusesDbContext = dbContext.ServiceStatuses;
        }
        public async Task<ServiceStatus?> GetServiceStatuses(int id)
        {
          return await _serviceStatusesDbContext.FirstOrDefaultAsync(a => a.Id == id);
        }

      
    }
}
