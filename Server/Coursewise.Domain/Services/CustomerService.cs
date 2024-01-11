using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Domain.Models.Dto;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class CustomerService : GenericService<Data.Entities.Customer, Domain.Entities.Customer, Guid>, ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPaymentService _paymentService;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<CoursewiseUser> _userManager;
        private readonly string linkedInAccessTokenUrl;
        private readonly string linkedInProfileUrl;
        private readonly string linkedInClientId;
        private readonly string linkedInSecret;
        private readonly string googleProfileUrl;
        private readonly string QuarterlyPriceId;
        private readonly string YearlyPriceId;
        private readonly string Yearly1MDiscountedPriceId;
        private readonly string Yearly2MDiscountedPriceId;
        public CustomerService(
            ICustomerRepository customerRepository,
            IPaymentService paymentService,
            IUtilityFacade utilityFacade,
            UserManager<CoursewiseUser> userManager,
            IWebHostEnvironment env,
            ICoursewiseLogger<CustomerService> logger,
            IConfiguration configuration
            ) : base(utilityFacade.Mapper, customerRepository, utilityFacade.UnitOfWork)
        {
            _userManager = userManager;
            _mapper = utilityFacade.Mapper;
            _customerRepository = customerRepository;
            _paymentService = paymentService;
            _env = env;
            linkedInAccessTokenUrl = configuration["Linkedin:AccessTokenUrl"];
            linkedInProfileUrl = configuration["Linkedin:ProfileUrl"];
            linkedInClientId = configuration["Linkedin:ClientId"];
            linkedInSecret = configuration["Linkedin:ClientSecret"];
            googleProfileUrl = configuration["Google:ProfileUrl"];
            QuarterlyPriceId = configuration["Stripe:QuarterlyPriceId"];
            YearlyPriceId = configuration["Stripe:YearlyPriceId"];
            Yearly1MDiscountedPriceId = configuration["Stripe:YearlyDiscounted1MPriceId"];
            Yearly2MDiscountedPriceId = configuration["Stripe:YearlyDiscounted2MPriceId"];


        }

        public async Task<Domain.Entities.Customer> GetByEmail(string email)
        {
            var user = (await _customerRepository.GetAsync(x => x.Email == email && x.UserId != null)).FirstOrDefault();
            return _mapper.Map<Domain.Entities.Customer>(user);
        }
        public async Task<bool> isFreeResourcesAvailble(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.FreeResourcesCount < 3)
            {
                return true;
            }
            return false;
        }
        public async Task<int> UpdateFreeResources(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.FreeResourcesCount < 3)
            {
                user.FreeResourcesCount = user.FreeResourcesCount + 1;
                await _userManager.UpdateAsync(user);
                return 3-user.FreeResourcesCount;
            }
            return 0;
        }
        public async Task<CoursewiseUser> GetUserById(string? userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<Domain.Entities.Customer> GetByUserId(string userId)
        {
            var user = (await _customerRepository.GetAsync(x =>x.UserId!=null&& x.UserId == userId)).FirstOrDefault();
            return _mapper.Map<Domain.Entities.Customer>(user);
        }
        public async Task<bool> isSubscribed(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var customer = await this.GetByUserId(user.Id);
                if (DateTime.Now < customer.MembershipExpiryDate && customer.isMember)
                {
                    return true;
                }
                if (DateTime.Now < customer.OneToOneExpiryDate && customer.IsOneToOneMember)
                {
                    return true;
                }
                var customerService = new Stripe.CustomerService();
                var customers = await customerService.ListAsync(new CustomerListOptions { Email = user.Email.ToLower() });
                if (customers.Any())
                {
                    var subscriptionService = new Stripe.SubscriptionService();

                    var subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions { Customer = customers.First().Id });
                    if (subscriptions.Any())
                    {
                       return GetIsSubscribed(subscriptions);


                    }
                }
            }
            return false;
        }
        private bool GetIsSubscribed(StripeList<Subscription> subscriptions)
        {

            var membershipSub = subscriptions.FirstOrDefault(a => a.Items.Data.Find(a =>
                        a.Price.Id == QuarterlyPriceId || a.Price.Id == YearlyPriceId
                        || a.Price.Id == Yearly1MDiscountedPriceId || a.Price.Id == Yearly2MDiscountedPriceId) != null);
            if (membershipSub != null && membershipSub.CurrentPeriodEnd > DateTime.Now)
            {

                return true;

            }
            return false;
        }
        public async Task<BaseModel> UpdateNotifications(string? userId, CustomerNotifications model)
        {
            string errorMessage = "User not found";
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.JobNotification = model.jobNotification;
                    user.ResourceNotification = model.resourceNotification;
                    await _userManager.UpdateAsync(user);
                    return BaseModel.Success("User notification settings updated");
                }
            }
            return BaseModel.Error(errorMessage);

        }
        public  async Task<CoursewiseUser?> LoginWithLinkedin(ExternalLoginModel model)
        {

           var response =await GetLinkedInInformationAsync(model.AccessToken);
            if (response != null)
            {
                return await VerifyExternalLogin(response, model);
            }
            return null;
        }
        public async Task<CoursewiseUser?> LoginWithGoogle(ExternalLoginModel model)
        {

            var response = await GetGoogleInformationAsync(model.AccessToken);
            if (response != null)
            {
                return await VerifyExternalLogin(response, model);
            }
            return null;
        }
        private async Task<CoursewiseUser?> VerifyExternalLogin(ExternalLoginRegisterModel response, ExternalLoginModel model)
        {

            string email = response.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var logins = await _userManager.GetLoginsAsync(user);
                if (!logins.Any(a => a.ProviderKey.Equals(response.Id) && a.LoginProvider.Equals(model.Provider)))
                {
                    await _userManager.AddLoginAsync(user, new UserLoginInfo(model.Provider, response.Id.ToString(), model.Provider ));

                }
                return user;
            }
            else
            {
                var newUser = new CoursewiseUser
                {
                    Name = response.FirstName + " " + response.LastName,
                    UserName = response.Email,
                    Email = response.Email,
                    EmailConfirmed = true

                };

                await _userManager.CreateAsync(newUser);
                await _userManager.AddLoginAsync(newUser, new UserLoginInfo(model.Provider, model.Provider, response.Id));

                var roleClaim = new Claim(ClaimTypes.Role, CoursewiseRoles.CUSTOMER);
                await _userManager.AddClaimAsync(newUser, roleClaim);
                await this.Add(new Domain.Entities.Customer
                {
                    Email = response.Email,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    UserId = newUser.Id,
                    isMember = false,
                    MembershipExpiryDate = DateTime.Now,
                    WordpressBoardwiseUserId = 0

                });
                return newUser;
            }
        }
        public async Task<BaseModel> GetLinkedinToken(LinkedinAccessTokenRequest request)
        {

            var errorMessage ="There was a problem getting access token";
            var url = linkedInAccessTokenUrl;
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", request.grant_type));
            nvc.Add(new KeyValuePair<string, string>("code", request.code));
            nvc.Add(new KeyValuePair<string, string>("client_id", linkedInClientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", linkedInSecret));
            nvc.Add(new KeyValuePair<string, string>("redirect_uri", request.redirect_uri));
            try { 
            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(nvc))
                {

                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    var contents = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<dynamic>(contents);
                        if (obj != null)
                        {
                            return BaseModel.Success(new { access_token = obj.access_token.ToString() });
                        }
                        else
                        {
                            return BaseModel.Error(errorMessage);

                        }

                    }
            }
            
            }
            catch
            {
                return BaseModel.Error(errorMessage);
            }
        }
        private async Task<ExternalLoginRegisterModel?> GetGoogleInformationAsync(string accesstoken)
        {
            var url = googleProfileUrl;

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

                HttpResponseMessage response = await httpClient.GetAsync(url);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse != null)
                {
                    var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (jsonObj != null)
                    {
                        ExternalLoginRegisterModel regModel = new ExternalLoginRegisterModel();
                        regModel.FirstName = jsonObj.given_name.ToString();
                        regModel.LastName = jsonObj.family_name.ToString();
                        regModel.Email = jsonObj.email.ToString();
                        regModel.Id = jsonObj.id.ToString();
                        return regModel;
                    }


                }

                return null;

            }
        }

        private async Task<ExternalLoginRegisterModel?> GetLinkedInInformationAsync(string accesstoken)
        {
            var url = linkedInProfileUrl;
          
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

                HttpResponseMessage response = await httpClient.GetAsync(url);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse != null)
                {
                    var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (jsonObj != null)
                    {
                        ExternalLoginRegisterModel regModel = new ExternalLoginRegisterModel();
                        regModel.FirstName = jsonObj.given_name.ToString();
                        regModel.LastName = jsonObj.family_name.ToString();
                        regModel.Email = jsonObj.email.ToString();
                        regModel.Id = jsonObj.sub.ToString();
                        return regModel;
                    }


                }

                return null;

            }
        }
        public async Task<BaseModel> GetBasicInfo(Guid customerId)
        {
            string errorMessage = "User does not exist";
            var customer= await _customerRepository.Get().FirstOrDefaultAsync(a=>a.Id==customerId);
            var domainCustomer = _mapper.Map<Domain.Entities.Customer>(customer);
            if (customer != null)
            {
                var user = await this.GetUserById(customer.UserId);
                var membership =await _paymentService.GetMembershipDetails(user, domainCustomer);
                var basicInfo = new
                {
                    customer.Id,
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.AutoRenewMembership,
                    customer.isMember,
                    user.JobNotification,
                    user.ResourceNotification,
                    ExpiryDate=customer.MembershipExpiryDate.ToString("yyyy-MM-dd"),
                    Membership=membership

                };
                return BaseModel.Success(basicInfo);

            }

            return BaseModel.Error(errorMessage);
           
        }
        public async Task<BaseModel> GetUsersfromCSV()
        {
            var path = _env.WebRootPath+"/CSV/importUsers.csv";
            var reader = new StreamReader(path);
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
                var records = csv.GetRecords<dynamic>();
            foreach (var record in records)
            {
                var email = record.user_email;
                var user = await this.GetByEmail(email);
                if (user == null)
                {
                    var newUser = new CoursewiseUser
                    {
                        Name = record.first_name + " " + record.last_name,
                        UserName = record.user_email,
                        Email = record.user_email,
                        EmailConfirmed = true

                    };

                     await _userManager.CreateAsync(newUser);
                    var roleClaim = new Claim(ClaimTypes.Role, CoursewiseRoles.CUSTOMER);
                    await _userManager.AddClaimAsync(newUser, roleClaim);
                    bool isMember = false;
                    if (record.roles.ToString().Contains("s2member"))
                    {
                        isMember = true;
                    }
                    await this.Add(new Domain.Entities.Customer
                    {
                        Email = record.user_email,
                        FirstName = record.first_name,
                        LastName = record.last_name,
                        UserId = newUser.Id,
                        isMember = isMember,
                        MembershipExpiryDate = DateTime.Now.AddYears(1),
                        WordpressBoardwiseUserId =Convert.ToInt32(record.ID.ToString())

                    }); 
                }
                


            }


            return BaseModel.Success(records.ToString(), records.Count(), "Successfully Applied");

        }
        public async Task<BaseModel> VerifyUserClaims()
        {
            var usersids = _customerRepository.Get().Select(a=>a.UserId).ToList();
            foreach (var id in usersids)
            {
                if (id != null)
                {
                    var user =await _userManager.FindByIdAsync(id);
                    var claims = await _userManager.GetClaimsAsync(user);
                    if (claims.Count == 0)
                    {
                        var roleClaim = new Claim(ClaimTypes.Role, CoursewiseRoles.CUSTOMER);
                        await _userManager.AddClaimAsync(user, roleClaim);
                    }
                }
            }
            return BaseModel.Success();

        }
        public async Task<BaseModel> SubscribeOneToOne(SubscribeOneToOne subscribe)
        {
           var customer=await _customerRepository.FirstOrDefaultAsync(a => a.Id == subscribe.Id);
            customer.IsOneToOneMember = subscribe.Subscribe;
           await _customerRepository.UpdateAsync(customer);
           await unitOfWork.SaveChangesAsync();
            if (subscribe.Subscribe)
            {
                return BaseModel.Success("Subscribed successfuly.");
            }
            else
            {
                return BaseModel.Success("Unsubscribed successfuly.");

            }

        }
        public async Task<BaseModel> GetOneToOneUsers(OneToOneFilters filters)
        {
            var pageSize = 50;
            var skip = (filters.Page * pageSize)-pageSize;
            OneToOneResult result = new OneToOneResult();
            result.Pagination.CurrentPage = filters.Page;

            var customerQuery = _customerRepository.Get();
            if (!String.IsNullOrEmpty(filters.Email))
            {
                customerQuery=customerQuery.Where(a =>a.Email!=null&&a.Email.ToLower().Contains(filters.Email.ToLower()));
            }
            if (filters.userType=="subscribed")
            {
                customerQuery = customerQuery.Where(a => a.IsOneToOneMember);
            }
            if (filters.userType == "unsubscribed")
            {
                customerQuery = customerQuery.Where(a =>!a.IsOneToOneMember);
            }
            result.Pagination.TotalRecords = customerQuery.Count();
            result.Pagination.TotalPages =Convert.ToInt32(Math.Ceiling((double)(result.Pagination.TotalRecords / pageSize)))+1;
            result.Users= await customerQuery.Select(a => new UserInformartion
            {
                Name = a.FirstName + " " + a.LastName,
                Email = a.Email,
                Id = a.Id,
                IsOneToOneMember = a.IsOneToOneMember,
                OneToOneExpiryDate = a.OneToOneExpiryDate
            }).OrderBy(a=>a.Email).Skip(skip).Take(pageSize).ToListAsync();

            return BaseModel.Success(result);
        }


    }
}
