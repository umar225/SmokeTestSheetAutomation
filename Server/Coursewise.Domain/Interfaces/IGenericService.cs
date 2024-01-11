using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IGenericService<TBusinessModel, TKey>
    {
        Task<List<TBusinessModel>> Get();
        Task<TBusinessModel> Get(TKey id);
        Task<TBusinessModel> Add(TBusinessModel businessEntity);
        Task Update(TBusinessModel businessEntity);
        Task Delete(TBusinessModel businessEntity);
    }
}
