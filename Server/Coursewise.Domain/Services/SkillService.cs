using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class SkillService : GenericService<Data.Entities.Skill, Skill, int>, ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SkillService(
            ISkillRepository skillRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(mapper, skillRepository, unitOfWork)
        {
            _skillRepository = skillRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ValidSkills(List<int> ids)
        {
            var hasValidList = await _skillRepository.Get().CountAsync(x => ids.Contains(x.Id));
            return hasValidList == ids.Count;
        }

        public async Task<List<Skill>> GetCounts()
        {
            var count = await _skillRepository.Get().Include(l => l.JobSkills)
                                    .Select(l => new Skill { Id = l.Id, Name = l.Name, NoOfJob = l.JobSkills.Count() })
                                    .OrderByDescending(i => i.NoOfJob)
                                    .ToListAsync();
            return count;
        }

        public async Task<BaseModel> UpdateCounts()
        {
            var results = await _skillRepository.Get().Include(l => l.JobSkills).ToListAsync();
            foreach (var item in results)
            {
                item.NoOfJob = item.JobSkills.Count();
            }
            await _skillRepository.UpdateAsync(results);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }
    }
}
