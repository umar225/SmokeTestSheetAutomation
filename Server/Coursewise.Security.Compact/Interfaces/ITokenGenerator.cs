using Coursewise.Security.Compact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Interfaces
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Takes a list of claims and generates an access token
        /// </summary>
        /// <param name="claims">System.Security.Claims</param>
        /// <returns></returns>
        Task<AccessToken> GenerateToken(string userId, string email, IEnumerable<Claim> claims);
    }
}
