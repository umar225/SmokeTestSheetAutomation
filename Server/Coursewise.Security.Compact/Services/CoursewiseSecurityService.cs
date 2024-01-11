using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Security.Compact.Interfaces;
using Coursewise.Security.Compact.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Services
{
    public class CoursewiseSecurityService : ICoursewiseSecurityService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly UserManager<CoursewiseUser> _userManager;
        private readonly SignInManager<CoursewiseUser> _signInManager;
        private readonly string reCaptchaSecret;
        private readonly string reCaptchaURL;

        public CoursewiseSecurityService(
            IConfiguration configuration,
            ITokenGenerator tokenGenerator,
            UserManager<CoursewiseUser> userManager,            
            SignInManager<CoursewiseUser> signInManager)
        {
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
            _signInManager = signInManager;
            reCaptchaSecret = configuration["Google:reCaptchaSecret"];
            reCaptchaURL = configuration["Google:reCaptchaURL"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> Login(string email, string password, IEnumerable<Claim>? claims = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return AuthenticationResult.Fail("Your email and password do not match");

            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: true);
            if (result.IsLockedOut)
                return AuthenticationResult
                    .Fail("Your account has been temporarily locked. Please try again in 5 minutes.");

            if (result.Succeeded)
            {
                var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
                return AuthenticationResult.ClaimResult(user, userClaims);
            }

            return AuthenticationResult
                    .Fail("Your email and password do not match");

        }
       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="createActivated"></param>
        /// <param name="roleClaimValue"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> CreateUser(string name, string email, string password, bool createActivated, string roleClaimValue)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return AuthenticationResult.Fail("user already exists.");
            }

            var newUser = new CoursewiseUser() { Name = name, Email = email, UserName = email };

            if (createActivated)
                newUser.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(roleClaimValue))
                {
                    await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, roleClaimValue));
                }



                return new AuthenticationResult()
                {
                    Success = true,
                    Message = "User registered successfully.",
                    UserInfo = new UserInfo() { UserId = newUser.Id }
                };
            }

            return AuthenticationResult.Fail(
               result.Errors.FirstOrDefault() != null
                   ? result.Errors.FirstOrDefault()!.Description
                   : "failed to create a user");
        }
        /// <summary>
        /// Creates the specified user with no password
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="createActivated"></param>
        /// <param name="roleClaimValue"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> CreateUser(string name, string email, string roleClaimValue, bool createActivated)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return AuthenticationResult.Fail("user already exists.");
            }

            var newUser = new CoursewiseUser() { Name = name, Email = email, UserName = email };

            if (createActivated)
                newUser.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(roleClaimValue))
                {
                    await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, roleClaimValue));
                }



                return new AuthenticationResult()
                {
                    Success = true,
                    Message = "User registered successfully.",
                    UserInfo = new UserInfo() { UserId = newUser.Id }
                };
            }

            return AuthenticationResult.Fail(
               result.Errors.FirstOrDefault() != null
                   ? result.Errors.FirstOrDefault()!.Description
                   : "failed to create a user");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="createActivated"></param>
        /// <param name="roleClaimValue"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> CreateUser(string email, string password, bool createActivated, string roleClaimValue)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return AuthenticationResult.Fail("user already exists.");
            }

            var newUser = new CoursewiseUser() { Email = email, UserName = email };

            if (createActivated)
                newUser.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(roleClaimValue))
                    await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, roleClaimValue));

                return new AuthenticationResult()
                {
                    Success = true,
                    Message = "User registered successfully.",
                    UserInfo = new UserInfo() { UserId = newUser.Id }
                };
            }

            return AuthenticationResult.Fail(
               result.Errors.FirstOrDefault() != null
                   ? result.Errors.FirstOrDefault()!.Description
                   : "failed to create a user");


        }

        public virtual async Task<AuthenticationResult> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(email);
                if (user == null)
                {
                    return AuthenticationResult.Fail("No user exists with the specified email address");
                }
            }
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return
                    AuthenticationResult.Succeed(message: "Password changed AuthenticationResult.Succeedfully");
            }
            return AuthenticationResult.Fail("AuthenticationResult.Failed to change password");

        }

        /// <summary>
        /// Generate Password Reset Token.
        /// AuthenticationResult.Succeed : UserId:guid  
        ///           restCode:string
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns> 
        public virtual async Task<AuthenticationResult> GeneratePasswordResetToken(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user == null)
            {
                return AuthenticationResult.Fail("No user exists with the specified email");
            }

            var resetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            return AuthenticationResult.PasswordResetToken(user.Id, resetCode, user.Name);
        }

        /// <summary>
        /// Add Claim to the user.
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> AddUserClaim(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AuthenticationResult.Fail("user not found with specified Id");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any())
            {
                var givenClaim = userClaims.FirstOrDefault(x => x.Type.ToLower() == claimType.ToLower() && x.Value.ToLower() == claimValue.ToLower());

                if (givenClaim != null)
                {
                    return AuthenticationResult.Succeed(message: "The specified claim already assigned to user, try different value.");
                }
            }
            var result = await _userManager.AddClaimAsync(user, new Claim(claimType, claimValue));

            if (result.Succeeded)
            {
                return AuthenticationResult.Succeed(message: "Claim added");
            }

            var message = result.Errors.FirstOrDefault() != null
                ? result.Errors.FirstOrDefault()!.Description
                : "AuthenticationResult.Failed to add claim";

            return AuthenticationResult.Fail(message);
        }

        /// <summary>
        /// Removes Claim from the user
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> RemoveUserClaim(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AuthenticationResult.Fail("user not found with specified Id");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any())
            {
                var givenClaim = userClaims.FirstOrDefault(x => x.Type.ToLower() == claimType.ToLower() && x.Value.ToLower() == claimValue.ToLower());

                if (givenClaim == null)
                {
                    return AuthenticationResult.Succeed(message: "User doesn't have the specified claim.");

                }
            }

            var result = await _userManager.RemoveClaimAsync(user, new Claim(claimType, claimValue));

            if (result.Succeeded)
            {
                return AuthenticationResult.Succeed(message: "Claim removed AuthenticationResult.Succeedfully.");

            }

            return AuthenticationResult.Fail(result.Errors.FirstOrDefault() != null ?
                result.Errors.FirstOrDefault()!.Description : "AuthenticationResult.Failed to remove claim");
        }



        /// <summary>
        /// Generate Email verification token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> GenerateEmailVerificationToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return AuthenticationResult.Fail("No user exists with the specified email address");
            }

            var varficationCode = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

            if (string.IsNullOrWhiteSpace(varficationCode))
            {
                return AuthenticationResult.Fail("Email varification could not be generated.");
            }

            return AuthenticationResult.EmailVerification(varficationCode);
        }


        public async Task<bool> ValidateCaptcha(string encodedResponse)
        {
            if (string.IsNullOrEmpty(encodedResponse)) return false;


            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
               {"secret", reCaptchaSecret},
               {"response", encodedResponse}
            };
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(reCaptchaURL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);
                if (captchaResponse != null)
                {
                    return captchaResponse.success;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Verify Email of the user.
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> VerifyEmail(string userId, string verificationCode)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return AuthenticationResult.Fail("No user exists with the specified email address");
            }

            var response = await _userManager.ConfirmEmailAsync(user, verificationCode);

            if (response.Succeeded)
            {
                return AuthenticationResult.Succeed(message: "Email confirmed.");
            }
            var message = response.Errors.FirstOrDefault() != null
                ? response.Errors.FirstOrDefault()!.Description
                : "Email confirmation AuthenticationResult.Failed.";
            return
                AuthenticationResult.Fail(message);
        }


        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> ResetPassword(string email, string code, string password)
        {
            var user = await _userManager.FindByIdAsync(email);
            if (user == null)
            {
                return AuthenticationResult.Fail("No user exists with the specified user Id.");
            }


            var result = await _userManager.ResetPasswordAsync(user, code, password);

            if (result.Succeeded)
            {
                var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
                return AuthenticationResult.ClaimResult("Thanks! Your new password has been set. You can now sign into the Coursewise app with your new password.", user, userClaims);

            }

            return AuthenticationResult.Fail(
                result.Errors.FirstOrDefault() != null
                    ? result.Errors.FirstOrDefault()!.Description
                    : "Password reset AuthenticationResult.Failed");

        }

        /// <summary>
        /// Used to validate Password Reset Token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> ValidatePasswordResetToken(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (string.IsNullOrWhiteSpace(token))
            {
                return AuthenticationResult.Fail("Please provide a valid token to validate.");
            }
            if (user is null)
                return AuthenticationResult.Fail("Token is in-valid.");
            var result = await _userManager.VerifyUserTokenAsync(user, "Default", "ResetPassword", token);
            return result ? AuthenticationResult.Succeed("Token is valid.") : AuthenticationResult.Fail("Token is in-valid.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AuthenticationResult.Fail("invalid user Id.");
            }

            var result = await _userManager.ConfirmEmailAsync(user!, code);
            if (result.Succeeded)
            {
                return AuthenticationResult.Succeed("email confirmed");
            }

            return AuthenticationResult.Fail(
               result.Errors.FirstOrDefault() != null
                   ? result.Errors.FirstOrDefault()!.Description
                   : "failed to confirm email");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userClaims"></param>
        /// <param name="claims"></param>       
        /// <returns></returns>
        public async Task<AuthenticationResult> GetUserToken(CoursewiseUser user, List<Claim> userClaims, IEnumerable<Claim> claims,string role, object? infoObject=null)
        {
            if (claims != null)
            {
                userClaims.AddRange(claims);
            }

            var token = await _tokenGenerator.GenerateToken(user.Id, user.Email, userClaims);
            
            return AuthenticationResult.LoginResult(token, user.Id, user.Email, role, infoObject: infoObject!);

        }
        /// <summary>
        /// Set user password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>        
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> SetUserPassword(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AuthenticationResult.Fail("Information is not correct.");
            }
            var result = await _userManager.AddPasswordAsync(user, password);
            if (result.Succeeded)
            {
                return new AuthenticationResult()
                {
                    Success = true,
                    Message = "User registered successfully.",
                    UserInfo = new UserInfo() { UserId = user.Id }
                };
            }

            return AuthenticationResult.Fail(
               result.Errors.FirstOrDefault() != null
                   ? result.Errors.FirstOrDefault()!.Description
                   : "failed to register a user");
        }
    }
}
