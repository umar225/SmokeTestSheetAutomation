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
    public class TitleRepository : GenericRepository<Title, int>, ITitleRepository
    {
        
        public TitleRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
