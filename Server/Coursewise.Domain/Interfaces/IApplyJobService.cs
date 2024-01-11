using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IApplyJobService: IGenericService<CustomerJob, int>
    {
        Task<BaseModel> Apply(Models.Dto.ApplyJobDto model, string customerWpToken, Guid customerId);
    }
}
