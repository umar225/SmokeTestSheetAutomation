using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Domain.Extensions;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Domain.Models.Dto;
using Coursewise.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICourseService _courseService;
        private readonly ICoursewiseLogger<PaymentService> _logger;
        private readonly string AppUrl;
        private readonly string QuarterlyPriceId;
        private readonly string YearlyPriceId;
        private readonly string Yearly1MDiscountedPriceId;
        private readonly string Yearly2MDiscountedPriceId;
        private readonly UserManager<CoursewiseUser> _userManager;


        public PaymentService(IConfiguration configuration, ICourseService courseService,
             UserManager<CoursewiseUser> userManager,
            ICoursewiseLogger<PaymentService> logger)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            AppUrl = configuration["Nova:AppUrl"];
            QuarterlyPriceId = configuration["Stripe:QuarterlyPriceId"];
            YearlyPriceId = configuration["Stripe:YearlyPriceId"];
            Yearly1MDiscountedPriceId = configuration["Stripe:YearlyDiscounted1MPriceId"];
            Yearly2MDiscountedPriceId = configuration["Stripe:YearlyDiscounted2MPriceId"];
            _courseService = courseService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BaseModel> CreateSubscription(Models.SubscriptionCreate subscription)
        {

            var customerService = new Stripe.CustomerService();
            var customers = await customerService.ListAsync(new CustomerListOptions { Email = subscription.Email.ToLower() });
            if (customers.Any())
            {
                var customerId = customers.First().Id;
                var paymentMethods = GetPaymentMethods(customerId);
                if (!paymentMethods.Any() || !paymentMethods.Any(a => a.Id == subscription.PaymentMethodId))
                {
                    AttachPaymentMethod(subscription.PaymentMethodId, customerId);

                }
                ChangeDefaultPaymentMethod(subscription.PaymentMethodId, customerId);
                var subscriptionService = new Stripe.SubscriptionService();
                var options = new SubscriptionCreateOptions
                {
                    Customer = customers.First().Id,
                    Items = new List<SubscriptionItemOptions>
                    {
                    new SubscriptionItemOptions
                    {
                    Price =subscription.PriceId,
                    },
                    },
                };
                subscriptionService.Create(options);
                return BaseModel.Success();
            }
            return BaseModel.Error("Customer not found");

        }
        public async Task<BaseModel> CreateCheckout(CoursewiseUser? user, int subscriptionType)
        {
            string priceIdToSubscribe = "";
            if (subscriptionType == (int)SubscriptionType.Quarterly)
            {
                priceIdToSubscribe = QuarterlyPriceId;
            }
            if (subscriptionType == (int)SubscriptionType.Yearly)
            {
                priceIdToSubscribe = YearlyPriceId;
            }
            if (user != null)
            {

                string stripeCustomerId =await GetStripeCustomerId(user);
                var result = await HandleSubscriptionsCheckout(stripeCustomerId, subscriptionType);
                if (result == "error")
                {
                    return BaseModel.Error("User is already subscribed.");
                }
                else if (result != "")
                {
                    priceIdToSubscribe = result;
                }
            
            var options = new SessionCreateOptions
            {
                SuccessUrl = AppUrl + "/pricing?payment=success",
                CancelUrl = AppUrl + "/pricing?payment=failed",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = priceIdToSubscribe,
                        Quantity=1
                    },
                },
                Mode = "subscription",
                Customer = stripeCustomerId,
            };
            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return BaseModel.Success(session.Url);
        }
            else
                return BaseModel.Error("User not found");

        }

    public async Task<BaseModel> CancelSubscription(CoursewiseUser? user,Domain.Models.Dto.CancelSubscriptionDto cancelSubscription)
    {
            return await CancelAndReJoinMembership(user, cancelSubscription);

    }
    private async Task<BaseModel> CancelAndReJoinMembership(CoursewiseUser? user, Domain.Models.Dto.CancelSubscriptionDto cancelSubscription)
    {
        if (user != null)
        {
            string stripeCustomerId = await GetStripeCustomerId(user);

            var subscriptionService = new Stripe.SubscriptionService();
            var subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions { Customer = stripeCustomerId });
            if (cancelSubscription.SubscriptionType == (int)SubscriptionType.Quarterly)
            {
                var quarterlySubscription = subscriptions.FirstOrDefault(a => a.Items.Any(b => b.Price.Id == QuarterlyPriceId));
                if (quarterlySubscription != null)
                {

                    var options = new SubscriptionUpdateOptions { CancelAtPeriodEnd = cancelSubscription.isCancel };
                    var service = new SubscriptionService();
                    service.Update(quarterlySubscription.Id, options);                      
                }
            }
            if (cancelSubscription.SubscriptionType == (int)SubscriptionType.Yearly)
            {
                var yearlySubscription = subscriptions.FirstOrDefault(a => a.Items.Any(b => b.Price.Id == YearlyPriceId));
                if (yearlySubscription != null)
                {

                    var options = new SubscriptionUpdateOptions { CancelAtPeriodEnd = cancelSubscription.isCancel };
                    var service = new SubscriptionService();
                    service.Update(yearlySubscription.Id, options);
                      
                }
            }
            if (cancelSubscription.isCancel)
            {
                return BaseModel.Success("Subscription canceled");
            }
            else
            {
                return BaseModel.Success("Subscription rejoined");
            }
            }


        return BaseModel.Error("Subscription not found");
    }
    private async Task<string> HandleSubscriptionsCheckout(string customerId, int subscriptionType)
    {
        var subscriptionService = new SubscriptionService();
        var subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions { Customer = customerId });
        if (subscriptions.Any())
        {
            subscriptions = await CancelSubscriptionsCheckout(subscriptions, customerId);
            if (subscriptions.Any(a => a.Items.Data.Exists(a => a.Price.Id == YearlyPriceId)))
            {
                return "error";
            }
            if (subscriptions.Any(a => a.Items.Data.Exists(a => a.Price.Id == QuarterlyPriceId)))
            {
                if (subscriptionType == (int)SubscriptionType.Quarterly)
                {
                    return "error";
                }

                else if (subscriptionType == (int)SubscriptionType.Yearly)
                {
                    var quarterlySub = subscriptions.First(a => a.Items.Data.Exists(a => a.Price.Id == QuarterlyPriceId));
                    if (quarterlySub != null)
                    {
                        return GetYearlyPriceId(quarterlySub);
                    }
                }
            }

        }
        return "";
    }
    private string GetYearlyPriceId(Subscription quarterlySub)
    {

        if ((quarterlySub.CurrentPeriodEnd - DateTime.Now).TotalDays >= 60)
        {
            return Yearly2MDiscountedPriceId;
        }
        else if ((quarterlySub.CurrentPeriodEnd - DateTime.Now).TotalDays >= 30)
        {
            return Yearly1MDiscountedPriceId;
        }
        else
        {
            return YearlyPriceId;

        }

    }
    private static async Task<StripeList<Subscription>> CancelSubscriptionsCheckout(StripeList<Subscription> subscriptions, string customerId)
    {
        var subscriptionsToCancel = subscriptions.Where(a => a.CurrentPeriodEnd < DateTime.Today);
        var subscriptionService = new SubscriptionService();
        if (subscriptionsToCancel.Any())
        {
            foreach (var sub in subscriptionsToCancel)
            {
                await subscriptionService.CancelAsync(sub.Id);
            }
            subscriptions = new StripeList<Subscription>();
            subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions { Customer = customerId });

        }
        return subscriptions;
    }
    public async Task<BaseModel> GetPricingPageDetails(CoursewiseUser? user, Domain.Entities.Customer? coursewiseCustomer)
    {
        try
        {
            
            return BaseModel.Success(await GetMembershipDetails(user, coursewiseCustomer));
        }
        catch (Exception ex)
        {
            return BaseModel.Error(ex.Message);
        }


    }
    public  async Task<PricingPageDto> GetMembershipDetails(CoursewiseUser? user, Domain.Entities.Customer? coursewiseCustomer)
    {
        PricingPageDto pricing = new PricingPageDto();
        if (user != null)
        {
            string stripeCustomerId = await GetStripeCustomerId(user);

            var subscriptionService = new Stripe.SubscriptionService();
            var subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions { Customer = stripeCustomerId });
            if (subscriptions.Any())
            {
                subscriptions = await HandleDiscountedMemberships(subscriptions, stripeCustomerId);

                await CancelExtraSubscriptionsAsync(subscriptions);
                pricing = GetActiveMembership(subscriptions, pricing);

            }
                if (!pricing.IsYearlyMember && !pricing.IsQuartelylyMember && coursewiseCustomer != null && coursewiseCustomer.IsOneToOneMember && coursewiseCustomer.OneToOneExpiryDate > DateTime.Now)
                {

                    pricing.IsOneToOneMember = true;
                    pricing.OneToOneExpiryDate = coursewiseCustomer.OneToOneExpiryDate;


                }
                if (!pricing.IsYearlyMember && !pricing.IsQuartelylyMember&&!pricing.IsOneToOneMember && coursewiseCustomer != null && coursewiseCustomer.isMember && coursewiseCustomer.MembershipExpiryDate > DateTime.Now)
            {

                pricing.IsFreeMember = true;
                pricing.FreeExpiry = coursewiseCustomer.MembershipExpiryDate;


            }
        }
            return pricing;
    }
    private static async Task CancelExtraSubscriptionsAsync(StripeList<Subscription> subscriptions)
    {

        if (subscriptions.Count() > 1)
        {
            var subscriptionService = new Stripe.SubscriptionService();
            var latestSubscription = subscriptions.FirstOrDefault(a => a.CurrentPeriodStart == subscriptions.Max(a => a.CurrentPeriodStart));
            foreach (var sub in subscriptions)
            {

                if (latestSubscription != null && sub.Id != latestSubscription.Id && sub.Status != "canceled")
                {
                    await subscriptionService.CancelAsync(sub.Id);
                }
            }
        }
    }
    private PricingPageDto GetActiveMembership(StripeList<Subscription> subscriptions, PricingPageDto pricing)
    {
        var currentSubscription = subscriptions.FirstOrDefault(a => a.CurrentPeriodStart == subscriptions.Max(a => a.CurrentPeriodStart));
        if (currentSubscription != null && currentSubscription.CurrentPeriodEnd > DateTime.Now)
        {
            if (currentSubscription.Items.Any(a => a.Price.Id == QuarterlyPriceId))
            {
                pricing.IsQuartelylyMember = true;
                pricing.QuarterlyExpiry = currentSubscription.CurrentPeriodEnd;
                if (currentSubscription.Status == "active")
                {
                        pricing.IsMembershipActive = !currentSubscription.CancelAtPeriodEnd;
                    }
                }
            else if (currentSubscription.Items.Any(a => a.Price.Id == YearlyPriceId))
            {
                pricing.IsYearlyMember = true;
                pricing.YearlyExpiry = currentSubscription.CurrentPeriodEnd;
                if (currentSubscription.Status == "active")
                {
                    pricing.IsMembershipActive = !currentSubscription.CancelAtPeriodEnd;
                }
            }
        }
        return pricing;
    }
    private async Task<StripeList<Subscription>> HandleDiscountedMemberships(StripeList<Subscription> subscriptions, string customerId)
    {
        var subscriptionService = new Stripe.SubscriptionService();
        var discountedSubs = subscriptions.Where(a => a.Items.Data.Exists(b => b.Price.Id == Yearly1MDiscountedPriceId || b.Price.Id == Yearly2MDiscountedPriceId));
        var canceledDiscounted = false;

        if (discountedSubs.Any())
        {
            foreach (var discountedSub in discountedSubs)
            {
                var options = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                        {
                            new SubscriptionItemOptions { Id = discountedSub.Items.First().Id, Deleted = true },
                            new SubscriptionItemOptions { Price = YearlyPriceId },
                        },
                };
                subscriptionService.Update(discountedSub.Id, options);

                canceledDiscounted = true;


            }
            if (canceledDiscounted)
            {

                subscriptions = new StripeList<Subscription>();
                subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions { Customer = customerId });

            }
        }
        return subscriptions;
    }
    public async Task<BaseModel> Create(Models.Order order)
    {
        var verificationResponse = await VerifyOrder(order);
        if (!verificationResponse.success)
            return verificationResponse;

        if (string.IsNullOrEmpty(order.IntentId))
        {
            var response = await CreateIntent(order, GetAmountInCents((double)verificationResponse.data!));
            return response;
        }
        else
        {
            var response = await UpdateIntent(order, GetAmountInCents((double)verificationResponse.data!));
            return response;
        }
    }
    public async Task<BaseModel> CreateSubscriptionOrder(Models.SubscriptionInformation subscription)
    {
        var verificationResponse = await VerifySubscription(subscription);
        if (!verificationResponse.success)
            return verificationResponse;
        var response = await CreateSubscriptionIntent(subscription, Convert.ToInt64(verificationResponse.data));
        return response;

    }
    public async Task<BaseModel> GetPaymentIntent(string IntentId)
    {
        var paymentIntentService = new PaymentIntentService();
        var existingIntent = await paymentIntentService.GetAsync(IntentId);
        if (existingIntent == null)
        {
            return BaseModel.Error("Order information is not correct");
        }
        if (existingIntent.Status == "succeeded")
        {
            return BaseModel.Error("Order information is not correct, Kindly try again");
        }
        return BaseModel.Success(new { clientSecret = existingIntent.ClientSecret, intentId = existingIntent.Id });
    }

    public async Task<BaseModel> CreateCustomer(BillingDetails billingDetails)
    {
        var options = new CustomerCreateOptions
        {
            Email = billingDetails.Email.ToLower(),
            Name = billingDetails.Name,
            Phone = billingDetails.Number,
            Address = new AddressOptions
            {
                City = billingDetails.Address.City,
                State = billingDetails.Address.State,
                Country = billingDetails.Address.Country,
                PostalCode = billingDetails.Address.PostalCode,
                Line1 = billingDetails.Address.Line1,
                Line2 = billingDetails.Address.Line2,
            }

        };
        var service = new Stripe.CustomerService();
        var customers = await service.ListAsync(new CustomerListOptions { Email = billingDetails.Email });
        string customerId;
        if (!customers.Any())
        {
            var customer = await service.CreateAsync(options);
            customerId = customer.Id;

        }
        else
        {

            customerId = customers.FirstOrDefault()!.Id;

        }
        var paymentIntentService = new PaymentIntentService();
        var meta = new Dictionary<string, string>();

        meta.Add("Notes", billingDetails.Notes);
        meta.Add("Company", billingDetails.CompanyName);
        await paymentIntentService.UpdateAsync(billingDetails.IntentId, new PaymentIntentUpdateOptions
        {
            Customer = customerId,
            Metadata = meta,
            ReceiptEmail = billingDetails.Email,
        });
        return BaseModel.Success();
    }

    private static StripeList<PaymentMethod> GetPaymentMethods(string customerId)
    {
        var options = new PaymentMethodListOptions
        {
            Customer = customerId,
            Type = "card",
        };
        var service = new PaymentMethodService();
        return service.List(
           options);
    }
    private static void ChangeDefaultPaymentMethod(string paymentMethodId, string customerId)
    {

        var options = new CustomerUpdateOptions
        {
            InvoiceSettings = new CustomerInvoiceSettingsOptions()
            {
                DefaultPaymentMethod = paymentMethodId
            }
        };
        var service = new Stripe.CustomerService();
        service.Update(customerId, options);
    }
    private static void AttachPaymentMethod(string paymentMethodId, string customerId)
    {
        var options = new PaymentMethodAttachOptions
        {
            Customer = customerId,
        };
        var service = new PaymentMethodService();
        service.Attach(
          paymentMethodId,
          options);
    }
    public async Task<BaseModel> VerifyOrder(Models.Order order)
    {
        var orderProducts = order.Products.ToList();
        double totalAmount = order.GetProductsSum();
        var products = await GetOrderProducts(orderProducts);
        if (!products.Any())
        {
            return BaseModel.Error($"This product(s) does not exist any more, Kindly empty your basket then add new products.");
        }
        double totalSum = 0;
        foreach (var orderProduct in orderProducts)
        {
            double productTotal = 0;
            var product = products.Find(x => x.Id == orderProduct.Id);
            if (product == null)
            {
                return BaseModel.Error($"Some products in the basket does not exist any more, Kindly empty your basket then add new products.");
            }
            orderProduct.Setup(product);
            productTotal = GetProductPrice(product, orderProduct);
            totalSum += productTotal;
        }
        if (Math.Round(totalSum, 2) != Math.Round(totalAmount, 2))
        {
            _logger.Warn($"Amount mismatch detected, Expected {Math.Round(totalSum, 2)} but found {Math.Round(totalAmount, 2)}");
            return BaseModel.Error($"Product(s) price mismatched. Kindly update the basket");
        }
        return BaseModel.Success(Math.Round(totalSum, 2));
    }
    private static async Task<BaseModel> VerifySubscription(Models.SubscriptionInformation subscription)
    {
        var priceService = new Stripe.PriceService();
        var price = await priceService.GetAsync(subscription.PriceId);
        return BaseModel.Success(price.UnitAmountDecimal);
    }
    private async static Task<BaseModel> CreateIntent(Models.Order order, long amount)
    {
        var paymentIntentService = new PaymentIntentService();
        var meta = new Dictionary<string, string>();
        var descriptionBuilder = new StringBuilder();
        int productCounter = 0;
        foreach (var item in order.Products)
        {
            var description = String.IsNullOrEmpty(item.Provider) ? $"{item.Name} x {item.Quantity}" : $"{item.Name} by {item.Provider} x {item.Quantity}";
            if (order.Products.Last().Id == item.Id)
            {

                descriptionBuilder.Append(description);
            }
            else
            {
                descriptionBuilder.Append($"{description}, ");
            }
            meta.Add($"Product {++productCounter}", $"{description}");
        }
        var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = "gbp",
            Metadata = meta,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
            Description = descriptionBuilder.ToString(),
        });
        return BaseModel.Success(new { clientSecret = paymentIntent.ClientSecret, intentId = paymentIntent.Id });
    }
    private async static Task<BaseModel> CreateSubscriptionIntent(Models.SubscriptionInformation subscription, long amount)
    {
        var paymentIntentService = new PaymentIntentService();
        var meta = new Dictionary<string, string>();
        var productService = new Stripe.ProductService();
        var product = await productService.GetAsync(subscription.ProductId);
        meta.Add($"Subscription {1}", $"{product.Name}");


        var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = "gbp",
            Metadata = meta,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
            Description = product.Name,
        });
        return BaseModel.Success(new { clientSecret = paymentIntent.ClientSecret, intentId = paymentIntent.Id });
    }
    private async static Task<BaseModel> UpdateIntent(Models.Order order, long amount)
    {
        var paymentIntentService = new PaymentIntentService();
        var existingIntent = await paymentIntentService.GetAsync(order.IntentId);
        if (existingIntent == null)
        {
            return BaseModel.Error("Order information is not correct");
        }
        if (existingIntent.Status == "succeeded")
        {
            return BaseModel.Error("Order information is not correct, Kindly try again");
        }
        var descriptionBuilder = new StringBuilder();
        var meta = new Dictionary<string, string>();
        int productCounter = 0;
        foreach (var item in order.Products)
        {
            var description = String.IsNullOrEmpty(item.Provider) ? $"{item.Name} x {item.Quantity}" : $"{item.Name} by {item.Provider} x {item.Quantity}";
            if (order.Products.Last().Id == item.Id)
            {
                descriptionBuilder.Append($"{description}");
            }
            else
            {
                descriptionBuilder.Append($"{description}, ");
            }
            meta.Add($"Product {++productCounter}", $"{description}");
        }
        var paymentIntent = await paymentIntentService.UpdateAsync(order.IntentId, new PaymentIntentUpdateOptions
        {
            Amount = amount,
            Metadata = meta,
            Description = descriptionBuilder.ToString(),
        });
        return BaseModel.Success(new { clientSecret = paymentIntent.ClientSecret, intentId = paymentIntent.Id });
    }



    private async Task<List<CourseDto>> GetOrderProducts(List<Models.Product> orderProducts)
    {
        var orderProductsIds = orderProducts.Select(s => s.Id);
        var courses = await _courseService.Get(x => orderProductsIds.Contains(x.Id));
        return courses;
    }

    private static double GetProductPrice(CourseDto product, Models.Product orderProduct)
    {
        double productTotal = 0;

        productTotal = product.Price * orderProduct.Quantity;
        return productTotal;
    }

    private static long GetAmountInCents(double amount)
    {
        return Convert.ToInt32(amount * 100);
    }
    private async Task<string> GetStripeCustomerId(CoursewiseUser user)
    {

        if (user.StripeCustomerId != null)
        {
            return user.StripeCustomerId;
        }
        else
        {
            
            var customerService = new Stripe.CustomerService();
            var customers = await customerService.ListAsync(new CustomerListOptions { Email = user.Email.ToLower() });
            Customer customer;
            if (!customers.Any())
            {
                var customerOptions = new CustomerCreateOptions
                {
                    Email = user.Email.ToLower(),
                    Name = user.Name,
                };
                customer = await customerService.CreateAsync(customerOptions);

            }
            else
            {
                customer = customers.First();
            }
            user.StripeCustomerId = customer.Id;
            await _userManager.UpdateAsync(user);
            
            return user.StripeCustomerId;

        }

    }
}
}
