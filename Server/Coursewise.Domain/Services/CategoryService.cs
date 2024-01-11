using AutoMapper;
using Coursewise.Common.Models;
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
    public class CategoryService : GenericService<Data.Entities.Category, Category, Guid>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(mapper, categoryRepository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public new  async Task<BaseModel> Add(Category businessEntity)
        {
            businessEntity.IsVisible = true;
            businessEntity.IsDeleted = false;
            if (string.IsNullOrEmpty(businessEntity.Name))
            {
                return BaseModel.Error("Please provide category name");
            }
            if (businessEntity.Courses!=null && businessEntity.Courses.Any())
            {
                return BaseModel.Error("Courses cannot be added during category creation");
            }
            if (string.IsNullOrEmpty(businessEntity.DisplayName))
            {
                businessEntity.DisplayName = businessEntity.Name;
            }
            var existingCategory = await _categoryRepository.Get().AnyAsync(c=> (c.Name == businessEntity.Name || c.DisplayName == businessEntity.DisplayName) && !c.IsDeleted);
            if (existingCategory)
            {
                return BaseModel.Error("This category already exist with this name/display name");
            }
            var dataCategory = _mapper.Map<Data.Entities.Category>(businessEntity);
            await _categoryRepository.AddAsync(dataCategory);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success(dataCategory.Id);
        }
        public new async Task<BaseModel> Update(Category businessEntity)
        {
            if (string.IsNullOrEmpty(businessEntity.Name))
            {
                return BaseModel.Error("Please provide category name");
            }
            
            var existingCategory = await _categoryRepository.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == businessEntity.Id);
            if (existingCategory == null)
                {
                    return BaseModel.Error("Invalid category Id");
                }
            if (string.IsNullOrEmpty(businessEntity.DisplayName))
            {
                businessEntity.DisplayName = businessEntity.Name;
            }
            if (businessEntity.Name == existingCategory.Name && businessEntity.DisplayName == existingCategory.DisplayName)
            {
                return BaseModel.Success(existingCategory.Id);
            }
            var alreadyExistCategory = await _categoryRepository.Get().AnyAsync(c => (c.Name == businessEntity.Name || c.DisplayName == businessEntity.DisplayName) && !c.IsDeleted && c.Id != existingCategory.Id);
            if (alreadyExistCategory)
            {
                return BaseModel.Error("A category already exist with this name/display name");
            }
            existingCategory.Name = businessEntity.Name;
            existingCategory.DisplayName = businessEntity.DisplayName;
            await _categoryRepository.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success(existingCategory.Id);
            
        }
        public override async  Task<List<Category>> Get()
        {
            var dbcategories = await _categoryRepository.Get().Where(c => !c.IsDeleted && c.IsVisible).ToListAsync();
            var categories = _mapper.Map<List<Category>>(dbcategories);
            return categories;
        }

        public async Task<List<Category>> GetAll()
        {
            var dbcategories = await _categoryRepository.Get().Where(c => !c.IsDeleted).ToListAsync();
            var categories = _mapper.Map<List<Category>>(dbcategories);
            return categories;
        }

        public async Task<bool> IsExist(Guid categoryId)
        {
            var isExist = await _categoryRepository.Get().AnyAsync(c => !c.IsDeleted && c.Id== categoryId);
            return isExist;
        }

        public async Task<BaseModel> Toggle(Category categoryModel)
        {
            var category = await _categoryRepository.Get().Where(c => !c.IsDeleted && c.Id == categoryModel.Id)
                .Include(c=>c.Courses.Where(cour=>!cour.IsDeleted)).FirstOrDefaultAsync();
            if (category == null)
                return BaseModel.Error("No category exist");
            if (category.IsVisible== categoryModel.IsVisible)
            {
                var status = category.IsVisible ? "visible" : "hidden";
                return BaseModel.Success($"Category {category.Name} already {status}");
            }
            category.IsVisible = categoryModel.IsVisible;
            foreach (var item in category.Courses)
            {
                item.IsVisible = categoryModel.IsVisible;
            }
            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success(category.Id);
        }

    }
}
