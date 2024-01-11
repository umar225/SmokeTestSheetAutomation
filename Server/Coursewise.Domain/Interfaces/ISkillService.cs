using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface ISkillService : IGenericService<Skill, int>
    {
        Task<bool> ValidSkills(List<int> ids);
        Task<List<Skill>> GetCounts();
        Task<BaseModel> UpdateCounts();
    }
}
