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
    public class IndustryService: GenericService<Data.Entities.Industry, Industry, int>, IIndustryService
    {
        private readonly IIndustryRepository _industryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IndustryService(
            IIndustryRepository industryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(mapper, industryRepository, unitOfWork)
        {
            _industryRepository = industryRepository;
            _unitOfWork = unitOfWork;


        }

        public async Task<bool> ValidIndustries(List<int> ids)
        {
            var hasValidList = await _industryRepository.Get().CountAsync(x => ids.Contains(x.Id));
            return hasValidList == ids.Count;
        }

        public async Task<List<Industry>> GetCounts()
        {
            var count = await _industryRepository.Get().Include(l => l.JobIndustry)
                                    .Select(l => new Industry { Id = l.Id, Name = l.Name, NoOfJob = l.JobIndustry.Count() })
                                    .OrderByDescending(i => i.NoOfJob)
                                    .ToListAsync();
            return count;
        }

        public async Task<BaseModel> UpdateCounts()
        {
            var results = await _industryRepository.Get().Include(l => l.JobIndustry).ToListAsync();
            foreach (var item in results)
            {
                item.NoOfJob = item.JobIndustry.Count();
            }
            await _industryRepository.UpdateAsync(results);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }
    }
}
