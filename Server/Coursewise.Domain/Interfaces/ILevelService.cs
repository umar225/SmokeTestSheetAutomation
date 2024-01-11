using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface ILevelService : IGenericService<Level, Guid>
    {
        Task<bool> HasValidLevels(List<Guid> levelsIds);
    }
}
