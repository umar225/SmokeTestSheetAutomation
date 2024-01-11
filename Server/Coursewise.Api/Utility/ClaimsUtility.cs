using Coursewise.Domain.Entities;
using System.Security.Claims;

namespace Coursewise.Api.Utility
{
    public static class ClaimsUtility
    {
        public static List<Claim> GetClaims(Customer customer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimValues.CustomerId, customer.Id.ToString(), ClaimValueTypes.String),

            };
            return claims;
        }
    }
}
