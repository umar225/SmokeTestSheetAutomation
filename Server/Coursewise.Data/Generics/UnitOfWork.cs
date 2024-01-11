using Coursewise.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Generics
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ICoursewiseDbContext dbContext;
        public UnitOfWork(ICoursewiseDbContext context)
        {
            dbContext = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
        public IDbContextTransaction BeginTransaction()
        {
            var transaction = dbContext.DatabaseObject.BeginTransaction();
            return transaction;
        }
    }
}
