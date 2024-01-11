using Coursewise.Api.Strategies.Interfaces;
using Coursewise.Api.Strategies.Models;
using Coursewise.Api.Utility;
using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using System.Dynamic;

namespace Coursewise.Api.Strategies.Services
{
    public class CustomerLoginStrategy : ILoginStrategy
    {
        public async Task<LoginOutput> RoleLogin(LoginParams param)
        {
            var user = await param.CustomerService.GetByEmail(param.LoginModel.Email);
            if (user == null)
                return new LoginOutput { BaseModel = BaseModel.Error(param.ErrorMessage) };
            var output = new LoginOutput();
            output.Claims = ClaimsUtility.GetClaims(user);
            output.UserId = user.Id.ToString();            
            return output;
        }
    }
    public class SuperAdminLoginStrategy : ILoginStrategy
    {
        public async Task<LoginOutput> RoleLogin(LoginParams param)
        {
            var output = new LoginOutput();
            output.BaseModel = BaseModel.Success();
            output.UserId = new ExpandoObject();
            return await Task.FromResult(output);
        }
    }

    public class LoginStrategy : ILoginStrategyExecuter
    {
        public Dictionary<string, Func<ILoginStrategy>> ValidOperations => new Dictionary<string, Func<ILoginStrategy>>() {
            {CoursewiseRoles.CUSTOMER, new Func<ILoginStrategy>(() => new CustomerLoginStrategy()) },
            {CoursewiseRoles.ADMIN, new Func<ILoginStrategy>(() => new SuperAdminLoginStrategy()) },
            
        };

        public async Task<LoginOutput> Execute(LoginParams loginParams)
        {
            var strategy = ValidOperations[loginParams.Role]();
            return await strategy.RoleLogin(loginParams);
        }
    }
}
