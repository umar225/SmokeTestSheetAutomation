using Coursewise.Domain.Models;
using Coursewise.Security.Compact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IExternalLoginService
    {
        Task<AuthenticationResult> LoginAsync(string email,string password,string role, string provider);
        Task<AuthenticationResult> LoginWPBoardwiseUser(string email, string password, string role);
    }
}
