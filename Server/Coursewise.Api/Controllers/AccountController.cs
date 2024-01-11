using Coursewise.Api.Models;
using Coursewise.Api.Strategies.Interfaces;
using Coursewise.Api.Utility;
using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models.Dto;
using Coursewise.Logging;
using Coursewise.Security.Compact.Interfaces;
using Coursewise.Security.Compact.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Coursewise.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICoursewiseSecurityService _securityService;
        private readonly ILoginStrategyExecuter _loginStrategy;
        private readonly ICustomerService _customerService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly SignInManager<CoursewiseUser> _signInManager;

        private readonly UserManager<CoursewiseUser> _userManager;

        public AccountController(
            ICoursewiseLogger<AccountController> logger,
            ICoursewiseSecurityService securityService,
            ICustomerService customerService,
            ILoginStrategyExecuter loginStrategy,
            UserManager<CoursewiseUser> userManager,
            SignInManager<CoursewiseUser> signInManager,
            IEmailTemplateService emailTemplateService

            )
        {
            _securityService = securityService;
            _customerService = customerService;
            _loginStrategy = loginStrategy;
            _userManager = userManager;
            _emailTemplateService = emailTemplateService;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var errorMessage = "That email and password do not match.";

            var loginResponse = await _securityService.Login(model.Email, model.Password);

            if (!loginResponse.Success)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(BaseModel.Error(errorMessage));
                }
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    var token = await _securityService.GeneratePasswordResetToken(user.Email);
                    await _emailTemplateService.ResetPassword(user.Email, user.Name, token.Tokens.PasswordResetToken, user.Id);
                    return Ok(BaseModel.Error("Due to a security update you will have to reset your password. An email has been sent to you to reset your password. Kindly check your inbox."));

                }
                else {
                    return Ok(BaseModel.Error(errorMessage));
                }



            }
            else
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (!loginResponse.CoursewiseUser.EmailConfirmed)
                {
                    return Ok(BaseModel.Error("Your Email is not verified yet. An email has been sent to you for verification. Kindly check your inbox."));

                }
                var role = loginResponse.UserClaims.Find(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    role = "customer";
                }
                return await ExecuteLoginStrategy(role, errorMessage, model, loginResponse, user);
            }


        }
        private async Task<IActionResult> ExecuteLoginStrategy(string role, string errorMessage, LoginModel model, AuthenticationResult loginResponse, CoursewiseUser user)
        {
            var loginStrategy = await _loginStrategy.Execute(new Strategies.Models.LoginParams
            {

                CustomerService = _customerService,
                Role = role,
                ErrorMessage = errorMessage,
                LoginModel = model
            });
            if (loginStrategy.BaseModel != null && !loginStrategy.BaseModel.success)
            {
                return Ok(loginStrategy.BaseModel);
            }

            var tokenResponse = await _securityService.GetUserToken(loginResponse.CoursewiseUser, loginResponse.UserClaims, loginStrategy.Claims, role);
            var response = new
            {
                tokenResponse.Tokens.AccessToken,
                tokenResponse.UserInfo.Email,
                tokenResponse.UserInfo.Role,
                loginStrategy.UserId,
                user.Name
            };
            return Ok(BaseModel.Success(response));
        }
        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ResetModel model)
        {
            var errorMessage = "User does not exist.";

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.PasswordResetAt.HasValue)
                {
                    DateTime startTime = DateTime.Now;

                    TimeSpan span = startTime.Subtract(user.PasswordResetAt.Value);
                    int interval = 2;
                    if (span.TotalMinutes < interval)
                    {
                        var spanInt = interval - Convert.ToInt16(span.TotalMinutes);
                        if (spanInt == 0)
                        {
                            spanInt = 1;
                        }
                        errorMessage = "Password reset already requested. Please try again in " + spanInt;
                        string minutes = " minutes.";
                        if (spanInt == 1)
                        {
                            minutes = " minute.";
                        }
                        errorMessage = errorMessage + minutes;
                        return Ok(BaseModel.Error(errorMessage));

                    }
                }

                var token = await _securityService.GeneratePasswordResetToken(user.Email);
                await _emailTemplateService.ResetPassword(user.Email, user.Name, token.Tokens.PasswordResetToken, user.Id);
                user.PasswordResetAt = DateTime.Now;
                await _userManager.UpdateAsync(user);
                return Ok(BaseModel.Success("An email has been sent to you to reset your password. Kindly check your inbox."));


            }

            else
            {
                return Ok(BaseModel.Error(errorMessage));

            }

        }
        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResetModel model)
        {
            var errorMessage = "User does not exist.";

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.PasswordResetAt.HasValue)
                {
                    DateTime startTime = DateTime.Now;

                    TimeSpan span = startTime.Subtract(user.PasswordResetAt.Value);
                    int interval = 2;
                    if (span.TotalMinutes < interval)
                    {
                        var spanInt = interval - Convert.ToInt16(span.TotalMinutes);
                        string minutes = " minutes.";
                        if (spanInt == 0|| spanInt == 1)
                        {
                            spanInt = 1;
                            minutes = " minute.";

                        }
                        errorMessage = "Verification email already requested. Please try again in " + spanInt;
                      
                        errorMessage = errorMessage + minutes;
                        return Ok(BaseModel.Error(errorMessage));

                    }
                }
                if (user.EmailConfirmed)
                {
                    return Ok(BaseModel.Error("Email is already verified."));

                }
                var token = await _securityService.GenerateEmailVerificationToken(user.Email);
                await _emailTemplateService.ConfirmEmail(user.Email, user.Name, token.Tokens.EmailVerificationToken, user.Id);
                user.PasswordResetAt = DateTime.Now;
                await _userManager.UpdateAsync(user);
                return Ok(BaseModel.Success("A confirmation link is sent to you via email. Please verify your email using that link."));


            }

            else
            {
                return Ok(BaseModel.Error(errorMessage));

            }

        }
        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var errorMessage = "User does not exist.";

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {

                var response = await _securityService.ResetPassword(user.Id, model.Token, model.Password);
                if (response.Success)
                {
                    return Ok(BaseModel.Success("Password has been reset you can continue to login now."));
                }
                else
                {
                    return Ok(BaseModel.Error(response.Message));


                }

            }
            else
            {
                return Ok(BaseModel.Error(errorMessage));

            }

        }
        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
        {
            var errorMessage = "User does not exist.";

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {

                var response = await _securityService.ConfirmEmail(user.Id, model.Token);
                if (response.Success)
                {
                    return Ok(BaseModel.Success("Email has been verified you can continue to login now."));
                }
                else
                {
                    return Ok(BaseModel.Error(response.Message));


                }

            }
            else
            {
                return Ok(BaseModel.Error(errorMessage));

            }

        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var errorMessage = "User does not exist.";
            if (model.CurrentPassword.Equals(model.NewPassword))
            {
                return Ok(BaseModel.Error("Current password an new password cannot be the same."));

            }
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userId?.Value);

            if (user != null)
            {

                var response = await _securityService.ChangePassword(user.Email, model.CurrentPassword, model.NewPassword);
                if (response.Success)
                {
                    return Ok(BaseModel.Success("Password changed successfully."));
                }
                else
                {
                    return Ok(BaseModel.Error(response.Message));


                }

            }
            else
            {
                return Ok(BaseModel.Error(errorMessage));

            }

        }
        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var errorMessage = "User already exists.";
            var isCaptchValid=await _securityService.ValidateCaptcha(model.reCaptcha);
            if (!isCaptchValid)
            {

                return Ok(BaseModel.Error("Invalid reCaptcha. Please fill reCaptcha again."));

            }
            var email = model.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new CoursewiseUser
                {
                    Name = model.FirstName + " " + model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = false,

                };
                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(newUser, model.Password);

                    bool isMember = false;

                    await _customerService.Add(new Domain.Entities.Customer
                    {
                        Email = newUser.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserId = newUser.Id,
                        isMember = isMember,
                        WordpressBoardwiseUserId = 0

                    });
                    var roleClaim = new Claim(ClaimTypes.Role, CoursewiseRoles.CUSTOMER);
                    await _userManager.AddClaimAsync(newUser, roleClaim);
                    var verificationResponse = await _securityService.GenerateEmailVerificationToken(newUser.Email);
                    await _emailTemplateService.ConfirmEmail(newUser.Email, newUser.Name, verificationResponse.Tokens.EmailVerificationToken, newUser.Id);

                    return Ok(BaseModel.Success("A confirmation link is sent to you via email. Please verify your email using that link."));
                }


            }
            return Ok(BaseModel.Error(errorMessage));

        }
        [AllowAnonymous]
        [HttpPost("external/login")]
        public async Task<IActionResult> LoginExternal([FromBody] ExternalLoginModel model)
        {
            model.Provider = model.Provider.ToLower();
            var errorMessage = "There is an error while signing in with " + model.Provider;
            var user = new CoursewiseUser();
            if (model.Provider.Equals("linkedin"))
            {
                 user = await _customerService.LoginWithLinkedin(model);
               
            }
            if (model.Provider.Equals("google"))
            {
                user = await _customerService.LoginWithGoogle(model);

            }
            if (user != null)
            {
                var logins = await _userManager.GetLoginsAsync(user);
                var loginResponse = await _signInManager.ExternalLoginSignInAsync(model.Provider, logins.First(a => a.LoginProvider == model.Provider).ProviderKey, true);
                if (loginResponse.Succeeded)
                {
                    var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
           
                    var role = userClaims.Find(c => c.Type == ClaimTypes.Role)?.Value;
                    if (role == null)
                    {
                        role = "customer";
                    }
                    var loginStrategy = await _loginStrategy.Execute(new Strategies.Models.LoginParams
                    {

                        CustomerService = _customerService,
                        Role = role,
                        ErrorMessage = errorMessage,
                        LoginModel = new LoginModel { Email = user.Email }
                    }) ;
                    if (loginStrategy.BaseModel != null && !loginStrategy.BaseModel.success)
                    {
                        return Ok(loginStrategy.BaseModel);
                    }

                    var tokenResponse = await _securityService.GetUserToken(user, userClaims, loginStrategy.Claims, role);
                    var response = new
                    {
                        tokenResponse.Tokens.AccessToken,
                        tokenResponse.UserInfo.Email,
                        tokenResponse.UserInfo.Role,
                        loginStrategy.UserId,
                        user.Name
                    };
                    return Ok(BaseModel.Success(response));
                }
            }
            return Ok(BaseModel.Error(errorMessage));
        }
        [AllowAnonymous]
        [HttpPost("linkedin/accesstoken")]
        public async Task<IActionResult> GetAccessToken([FromForm] LinkedinAccessTokenRequest request)
        {

            return Ok(await _customerService.GetLinkedinToken(request));
        }
        [AllowAnonymous]
        [ValidateModelStateAttribute]
        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginModel model)
        {
            var errorMessage = "Your email and password do not match";
            var loginResponse = await _securityService.Login(model.Email, model.Password);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (!loginResponse.Success)
                return Ok(BaseModel.Error(loginResponse.Message));

            var role = loginResponse.UserClaims.Find(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == null || role.ToLower() != "admin")
            {
                return Ok(new BaseModel(false, errorMessage));
            }
            else
            {

                return await ExecuteLoginStrategy(role, errorMessage, model, loginResponse, user);

            }
        }
       


    }
}


