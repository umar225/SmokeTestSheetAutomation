using AutoMapper;
using Coursewise.Data.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services.Facade
{
    public interface IUtilityFacade
    {
        IMapper Mapper { get; }
        IUnitOfWork UnitOfWork { get; }
    }
    public class UtilityFacade : IUtilityFacade
    {
        public IMapper Mapper { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }

        public UtilityFacade(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }
    }
}
