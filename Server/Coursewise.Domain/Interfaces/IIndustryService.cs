using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IIndustryService : IGenericService<Industry, int>
    {
        Task<bool> ValidIndustries(List<int> ids);
        Task<List<Industry>> GetCounts();
        Task<BaseModel> UpdateCounts();
    }
}
