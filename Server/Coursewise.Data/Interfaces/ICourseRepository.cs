using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course, Guid>
    {
        Task<List<Course>> GetByType(string name);
        Task<List<Course>> GetReadableByCategoryAndLevel(string category, string level);
    }
}
