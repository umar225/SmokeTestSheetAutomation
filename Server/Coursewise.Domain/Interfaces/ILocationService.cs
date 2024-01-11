using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface ILocationService: IGenericService<Location, int>
    {
        Task<bool> ValidLocations(List<int> ids);
        Task<List<Location>> GetCounts();
        Task<BaseModel> UpdateCounts();
    }
}
