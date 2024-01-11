using Coursewise.Common.Models;
using Coursewise.Domain.Models;
using Coursewise.Domain.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<BaseModel> Create(Models.Order order);
        Task<BaseModel> GetPaymentIntent(string IntentId);
        Task<BaseModel> CreateCustomer(BillingDetails billingDetails);
        Task<BaseModel> VerifyOrder(Models.Order order);
        Task<BaseModel> CreateSubscriptionOrder(Models.SubscriptionInformation subscription);
        Task<BaseModel> CreateSubscription(Models.SubscriptionCreate subscription);
        Task<BaseModel> CreateCheckout(CoursewiseUser? user, int subscriptionType);
        Task<BaseModel> GetPricingPageDetails(CoursewiseUser? user, Domain.Entities.Customer? coursewiseCustomer);
        Task<BaseModel> CancelSubscription(CoursewiseUser? user, Domain.Models.Dto.CancelSubscriptionDto cancelSubscription);
        Task<PricingPageDto> GetMembershipDetails(CoursewiseUser? user, Domain.Entities.Customer? coursewiseCustomer);
    }
}
