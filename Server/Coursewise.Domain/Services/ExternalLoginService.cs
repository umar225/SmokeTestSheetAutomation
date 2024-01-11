using Coursewise.Common.Exceptions;
using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Logging;
using Coursewise.Security.Compact.Interfaces;
using Coursewise.Security.Compact.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class ExternalLoginService:IExternalLoginService
    {
        private readonly UserManager<CoursewiseUser> _userManager;
        private readonly string _boardwiseWPUrl;
        private readonly ICoursewiseSecurityService _securityService;
        private readonly ICustomerService _customerService;
        private readonly ICoursewiseLogger<ExternalLoginService> _logger;

        public ExternalLoginService(
            UserManager<CoursewiseUser> userManager,
            ICoursewiseSecurityService securityService,
            ICustomerService customerService,
            ICoursewiseLogger<ExternalLoginService> logger,
            IConfiguration configuration
            )
        {
            _boardwiseWPUrl = configuration["Nova:BoardwiseWPUrl"];
            _userManager = userManager;
            _securityService = securityService;
            _customerService = customerService;
            _logger = logger;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password, string role, string provider)
        {
            var loginResponse = new AuthenticationResult();
            if (provider== "BoardwiseWordpress")
            {
                loginResponse = await LoginWPBoardwiseUser(email, password, role);
                if (!loginResponse.Success)
                {
                    return loginResponse;
                }
            }

            if (!loginResponse.Success)
            {
                var message = string.IsNullOrEmpty(loginResponse.Message) ? "This provider is not valid" : loginResponse.Message;
                return AuthenticationResult.Fail(message);
            }

            var checkExstingLoginResponse = await ExistingUserLogin(email, loginResponse);
            if (checkExstingLoginResponse.Success && checkExstingLoginResponse.Message != "No exisitng user exist")
            {
                return checkExstingLoginResponse;
            }
            var newUserLoginResponse = await SetupUserAndLogin(email, role, provider, loginResponse);
            return newUserLoginResponse;

        }

        public async Task<AuthenticationResult> LoginWPBoardwiseUser(string email, string password,string role)
        {
            using (var httpClient = new HttpClient())
            {
                Uri uri = new Uri($"{_boardwiseWPUrl}/wp-json/jwt-auth/v1/token");
                var headers = new Dictionary<string, string>();
                var body = new
                {
                    username = email,
                    password = password,
                };
                var request = HttpRequestMessageBuilder.Build(uri, HttpMethod.Post, body, headers);
                var response = await httpClient.PostAsync(uri,request.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.Error($"Login is not working for {email} due to wordpress server error");
                    return AuthenticationResult.Fail($"Login is not working for {email}");
                }

                var responseInString = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(responseInString);
                bool status = data.Value<bool>("success");
                if (!status)
                {
                    return AuthenticationResult.Fail(data!.Value<string>("message")!);
                }

                var boardwiseUserWpResponse = JsonConvert.DeserializeObject<BoardwiseUserWpResponse>(responseInString);
                if (boardwiseUserWpResponse != null && boardwiseUserWpResponse.Success && boardwiseUserWpResponse.Code== "jwt_auth_valid_credential")
                {
                    string name = string.IsNullOrEmpty(boardwiseUserWpResponse.User!.LastName) ? $"{boardwiseUserWpResponse.User.FirstName}" : $"{boardwiseUserWpResponse.User.FirstName} {boardwiseUserWpResponse.User.LastName}";
                    return AuthenticationResult.LoginResult(name,
                        boardwiseUserWpResponse.User.Email, role, boardwiseUserWpResponse, boardwiseUserWpResponse.User.Token);
                }
                return AuthenticationResult.Fail(boardwiseUserWpResponse!.Message);

            }
        }

        public async Task<AuthenticationResult> ExistingUserLogin(string email, AuthenticationResult loginResponse)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                var existingCustomer = await _customerService.GetByEmail(existingUser.Email);
                //** generate token
                var claims = await _userManager.GetClaimsAsync(existingUser);
                existingUser.Id = existingCustomer.Id.ToString();
                var response = await _securityService.GetUserToken(
                    user: existingUser,
                    userClaims: claims.ToList(), claims: new List<Claim>{
                        new Claim("customerId",existingCustomer.Id.ToString(), ClaimValueTypes.String),
                        new Claim("boardwiseWPToken",loginResponse.Tokens.AccessToken.Token, ClaimValueTypes.String),
                    },"", loginResponse.UserInfo.Data);

                return response;
            }
            return AuthenticationResult.Succeed("No exisitng user exist");
        }

        public async Task<AuthenticationResult> SetupUserAndLogin(string email,string role,string provider, AuthenticationResult loginResponse)
        {
            var newUser = new CoursewiseUser
            {
                Name = loginResponse.UserInfo.Name,
                UserName = email,
                Email = email,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(newUser);
            var boardwiseUserWpResponse = (BoardwiseUserWpResponse)(loginResponse.UserInfo.Data);
            var newCustomer = await _customerService.Add(new Entities.Customer
            {
                Email = email,
                FirstName = boardwiseUserWpResponse.User!.FirstName,
                LastName = boardwiseUserWpResponse.User.LastName,
                UserId = newUser.Id,
                WordpressBoardwiseUserId = boardwiseUserWpResponse.User.Id
            });

            if (!result.Succeeded)
                throw new CoursewiseBaseException(result.Errors.FirstOrDefault()!.Description);


            //** add role claim
            var roleClaim = new Claim(ClaimTypes.Role, role);
            await _userManager.AddClaimAsync(newUser, roleClaim);

            //** create external login
            var userLoginInfo = new UserLoginInfo(provider, boardwiseUserWpResponse.User.Id.ToString(), provider);
            result = await _userManager.AddLoginAsync(newUser, userLoginInfo);
            if (!result.Succeeded)
                throw new CoursewiseBaseException(result.Errors.FirstOrDefault()!.Description);

            //** generate token

            var userclaims = await _userManager.GetClaimsAsync(newUser);
            newUser.Id = newCustomer.Id.ToString();
            var tokenresponse = await _securityService.GetUserToken(
                user: newUser,
                userClaims: userclaims.ToList(), claims: new List<Claim>{
                        new Claim("customerId",newCustomer.Id.ToString(), ClaimValueTypes.String),
                        new Claim("boardwiseWPToken",loginResponse.Tokens.AccessToken.Token, ClaimValueTypes.String),
                    },"", loginResponse.UserInfo.Data);
            return tokenresponse;
        }
    }
}
