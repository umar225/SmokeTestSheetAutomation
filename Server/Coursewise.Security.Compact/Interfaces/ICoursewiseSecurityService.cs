using Coursewise.Common.Models;
using Coursewise.Security.Compact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Interfaces
{
    public interface ICoursewiseSecurityService
    {
        Task<AuthenticationResult> CreateUser(string name, string email, string password, bool createActivated, string roleClaimValue);
        /// <summary>
        /// Creates the specified user with no password
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="createActivated"></param>
        /// <param name="roleClaimValue"></param>
        /// <returns></returns>
        Task<AuthenticationResult> CreateUser(string name, string email, string roleClaimValue, bool createActivated);
        Task<AuthenticationResult> CreateUser(string email, string password, bool createActivated, string roleClaimValue);
        Task<AuthenticationResult> GeneratePasswordResetToken(string userName);

        Task<AuthenticationResult> ChangePassword(string email, string currentPassword, string newPassword);

        Task<AuthenticationResult> AddUserClaim(string userId, string claimType, string claimValue);

        Task<AuthenticationResult> RemoveUserClaim(string userId, string claimType, string claimValue);


        Task<AuthenticationResult> GenerateEmailVerificationToken(string email);

        Task<AuthenticationResult> ConfirmEmail(string userId, string code);



        Task<AuthenticationResult> Login(string email, string password, IEnumerable<Claim>? claims = null);
        Task<AuthenticationResult> GetUserToken(CoursewiseUser user, List<Claim> userClaims, IEnumerable<Claim> claims,string role, object? infoObject = null);

        Task<AuthenticationResult> ResetPassword(string email, string code, string password);
        Task<AuthenticationResult> ValidatePasswordResetToken(string userId, string token);
        /// <summary>
        /// Set user password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>        
        /// <returns></returns>
        Task<AuthenticationResult> SetUserPassword(string userId, string password);
        Task<bool> ValidateCaptcha(string encodedResponse);
    }
}
