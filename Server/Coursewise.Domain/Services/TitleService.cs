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
    public class TitleService : GenericService<Data.Entities.Title, Title, int>, ITitleService
    {
        private readonly ITitleRepository _titleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TitleService(
            ITitleRepository titleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(mapper, titleRepository, unitOfWork)
        {
            _titleRepository = titleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ValidSkills(List<int> ids)
        {
            var hasValidList = await _titleRepository.Get().CountAsync(x => ids.Contains(x.TitleId));
            return hasValidList == ids.Count;
        }

        public async Task<List<Models.Dto.TitleDto>> GetCounts()
        {
            var count = await _titleRepository.Get().Include(l => l.JobTitles)
                                    .Select(l => new Models.Dto.TitleDto { Id = l.TitleId, Name = l.Name, NoOfJob = l.JobTitles.Count() })
                                    .OrderByDescending(i => i.NoOfJob)
                                    .ToListAsync();
            return count;
        }

        public async Task<BaseModel> UpdateCounts()
        {
            var results = await _titleRepository.Get().Include(l => l.JobTitles).ToListAsync();
            foreach (var item in results)
            {
                item.NoOfJob = item.JobTitles.Count();
            }
            await _titleRepository.UpdateAsync(results);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }
    
    }
}
