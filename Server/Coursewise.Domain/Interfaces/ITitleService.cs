using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface ITitleService : IGenericService<Title, int>
    {
        Task<bool> ValidSkills(List<int> ids);
        Task<List<Models.Dto.TitleDto>> GetCounts();
        Task<BaseModel> UpdateCounts();
    }
}
