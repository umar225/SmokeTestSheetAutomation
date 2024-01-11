using Coursewise.Api.Strategies.Models;

namespace Coursewise.Api.Strategies.Interfaces
{
    public interface ILoginStrategy
    {
        Task<LoginOutput> RoleLogin(LoginParams param);
    }
    public interface ILoginStrategyExecuter
    {
        Task<LoginOutput> Execute(LoginParams loginParams);
    }
}
