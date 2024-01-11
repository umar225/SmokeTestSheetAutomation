using Coursewise.Common.Models;
using System.Security.Claims;

namespace Coursewise.Api.Strategies.Models
{
    public class LoginOutput
    {
        public BaseModel BaseModel { get; set; }
        public List<Claim> Claims { get; set; }
        public dynamic UserId { get; set; }        
    }
}
