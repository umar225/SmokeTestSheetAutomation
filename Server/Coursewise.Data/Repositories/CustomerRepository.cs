using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
