using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Models;
using Coursewise.Domain.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface ICustomerService : IGenericService<Customer, Guid>
    {
        Task<Customer> GetByEmail(string email);
        Task<BaseModel> GetUsersfromCSV();
        Task<BaseModel> VerifyUserClaims();
        Task<BaseModel> GetBasicInfo(Guid customerId);
        Task<BaseModel> UpdateNotifications(string? userId, CustomerNotifications model);
        Task<CoursewiseUser?> LoginWithLinkedin(ExternalLoginModel model);
        Task<CoursewiseUser?> LoginWithGoogle(ExternalLoginModel model);
        Task<BaseModel> GetLinkedinToken(LinkedinAccessTokenRequest request);
        Task<Customer> GetByUserId(string userId);
        Task<bool> isSubscribed(string? userId);
        Task<CoursewiseUser> GetUserById(string? userId);
        Task<bool> isFreeResourcesAvailble(string? userId);
        Task<int> UpdateFreeResources(string? userId);
        Task<BaseModel> GetOneToOneUsers(OneToOneFilters filters);
        Task<BaseModel> SubscribeOneToOne(SubscribeOneToOne subscribe);
    }
}
