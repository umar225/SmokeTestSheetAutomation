using Coursewise.Common.Models;
using Coursewise.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces.External
{
    public interface IWordpressService
    {
        Task<BaseModel> GetUserInfo(string token);
        Task<BaseModel> IsPaidUser(string token);
        Task<(BaseModel, List<BoardwiseWordpressUserBasic>)> GetAllUsers(string token);
    }
}
