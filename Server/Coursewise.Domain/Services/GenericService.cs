using AutoMapper;
using Coursewise.Data.Generics;
using Coursewise.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class GenericService<TDataModel, TBusinessModel, TKey> : IGenericService<TBusinessModel, TKey>
        where TBusinessModel : class where TDataModel : class
    {
        protected readonly IGenericRepository<TDataModel, TKey> repository;
        protected readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GenericService(IMapper mapper, IGenericRepository<TDataModel, TKey> genericRepository, IUnitOfWork unitOfWork)
        {
            this.repository = genericRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public virtual async Task<List<TBusinessModel>> Get()
        {
            var dataEntities = await repository.GetAsync();
            var businessEntities = mapper.Map<List<TDataModel>, List<TBusinessModel>>(dataEntities);
            return businessEntities;
        }

        public virtual async Task<TBusinessModel> Get(TKey id)
        {
            var dataEntity = await repository.GetAsync(id);
            var businessEntity = mapper.Map<TDataModel, TBusinessModel>(dataEntity);
            return businessEntity;
        }
        public virtual async Task<TBusinessModel> Add(TBusinessModel businessEntity)
        {
            var dataEntity = mapper.Map<TBusinessModel, TDataModel>(businessEntity);
            dataEntity = await repository.AddAsync(dataEntity);
            if (unitOfWork != null)
            {
                await unitOfWork.SaveChangesAsync();
            }
            businessEntity = mapper.Map<TDataModel, TBusinessModel>(dataEntity);
            return businessEntity;
        }

        public virtual async Task Update(TBusinessModel businessEntity)
        {
            var dataEntity = mapper.Map<TBusinessModel, TDataModel>(businessEntity);
            await repository.UpdateAsync(dataEntity);
            if (unitOfWork != null)
            {
                await unitOfWork.SaveChangesAsync();
            }
        }

        public virtual async Task Delete(TBusinessModel businessEntity)
        {
            var dataEntity = mapper.Map<TBusinessModel, TDataModel>(businessEntity);
            await repository.DeleteAsync(dataEntity);
            if (unitOfWork != null)
            {
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
