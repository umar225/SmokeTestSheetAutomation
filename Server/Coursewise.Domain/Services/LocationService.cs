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
    public class LocationService: GenericService<Data.Entities.Location, Location, int>, ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(mapper, locationRepository, unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;


        }

        public async Task<bool> ValidLocations(List<int> ids)
        {
            var hasValidList = await _locationRepository.Get().CountAsync(x => ids.Contains(x.Id));
            return hasValidList == ids.Count;
        }

        public async Task<List<Location>> GetCounts()
        {
            var locationsCount = await _locationRepository.Get().Include(l=>l.JobLocations)
                                    .Select(l=>new Location { Id=l.Id, Name=l.Name, NoOfJob=l.JobLocations.Count() } )
                                    .OrderByDescending(i => i.NoOfJob)
                                    .ToListAsync();
            return locationsCount;
        }

        public async Task<BaseModel> UpdateCounts()
        {
            var results = await _locationRepository.Get().Include(l => l.JobLocations).ToListAsync();
            foreach (var item in results)
            {
                item.NoOfJob = item.JobLocations.Count();
            }
            await _locationRepository.UpdateAsync(results);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }
    }
}
