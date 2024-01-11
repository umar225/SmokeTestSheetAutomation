using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;


namespace Coursewise.Data.Repositories
{
    public class ApplyJobRepository : GenericRepository<CustomerJob, int>, IApplyJobRepository
    {
        public ApplyJobRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
