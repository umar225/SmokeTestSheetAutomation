using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface ICategoryService:IGenericService<Category, Guid>
    {
        new Task<BaseModel> Add(Category businessEntity);
        new Task<BaseModel> Update(Category businessEntity);
        new Task<List<Category>> Get();
        Task<List<Category>> GetAll();
        Task<bool> IsExist(Guid categoryId);
        Task<BaseModel> Toggle(Category categoryModel);
    }
}
