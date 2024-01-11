using Coursewise.Api.Models;
using Coursewise.Domain.Interfaces;

namespace Coursewise.Api.Strategies.Models
{
    public class LoginParams
    {
        public string Role { get; set; }
        public string ErrorMessage { get; set; }
        public LoginModel LoginModel { get; set; }
        public ICustomerService CustomerService { get; set; }        
    }
}
