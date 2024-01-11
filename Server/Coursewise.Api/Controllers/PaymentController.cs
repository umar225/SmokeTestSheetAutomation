using Coursewise.Api.Models;
using Coursewise.Common.Models;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Coursewise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ICustomerService _customerService;

        public PaymentController(IConfiguration configuration,
            IPaymentService paymentService,
            ICustomerService customerService)
        {
            _paymentService = paymentService;
            _customerService = customerService;
        }
        [HttpPost]
        public async Task<ActionResult> Create(Order order)
        {
            var response = await _paymentService.Create(order);
            return Ok(response);
        }
        [HttpPost("Get/Intent")]
        public async Task<ActionResult> GetPaymentIntent(PaymentInformation payment)
        {
            var response = await _paymentService.GetPaymentIntent(payment.IntentId);
            return Ok(response);
        }
        [HttpPost("Get/SubscriptionIntent")]
        public async Task<ActionResult> GetSubscriptionIntent(SubscriptionInformation subscription)
        {
            var response = await _paymentService.CreateSubscriptionOrder(subscription);
            return Ok(response);
        }
        [HttpPost("CreateSubscription")]
        public async Task<ActionResult> CreateSubscription(SubscriptionCreate subscription)
        {
            var response = await _paymentService.CreateSubscription(subscription);

            return Ok(response);
        }
        [HttpGet("CreateCheckout")]
        public async Task<ActionResult> CreateCheckout(int subscriptionType)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var user = await _customerService.GetUserById(userId);

            var response = await _paymentService.CreateCheckout(user, subscriptionType);
            return Ok(response);
        }
        [HttpPost("CancelSubscription")]
        public async Task<ActionResult> CancelSubscription([FromForm] CancelSubscriptionDto cancelSubscription)
        {

            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var user = await _customerService.GetUserById(userId);

            var response = await _paymentService.CancelSubscription(user, cancelSubscription);
            return Ok(response);
        }
        [HttpGet("PricingDetails")]
        public async Task<ActionResult> GetPricingDetails()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var user = await _customerService.GetUserById(userId);
            var coursewiseCustomer = await _customerService.GetByEmail(user.Email);

            var response = await _paymentService.GetPricingPageDetails(user, coursewiseCustomer);
            return Ok(response);
        }
        [HttpPost("Customer")]
        public async Task<ActionResult> CreateCustomer(BillingDetails billingDetails)
        {
            var response = await _paymentService.CreateCustomer(billingDetails);
            return Ok(response);
        }
    }




    public class PaymentInformation
    {
        public string IntentId { get; set; }
    }


}
