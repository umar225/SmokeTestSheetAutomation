using AutoMapper;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class LevelService : GenericService<Data.Entities.Level, Level, Guid>, ILevelService
    {
        private readonly ILevelRepository _levelService;
        

        public LevelService(
            ILevelRepository levelService,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(mapper, levelService, unitOfWork)
        {
            _levelService = levelService;
            
        }

        public async Task<bool> HasValidLevels(List<Guid> levelsIds)
        {
            var hasValidLevel = await _levelService.Get().CountAsync(x => levelsIds.Contains(x.Id));
            return hasValidLevel==levelsIds.Count;
        }
    }
}
