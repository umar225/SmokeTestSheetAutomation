using Coursewise.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Models
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CoursewiseUser CoursewiseUser { get; set; }
        public List<Claim> UserClaims { get; set; }
        public UserInfo UserInfo { get; set; }
        public Tokens Tokens { get; set; }
        
        public AuthenticationResult()
        {
            UserInfo = new UserInfo();
            Tokens = new Tokens();
            CoursewiseUser = new CoursewiseUser();
            UserClaims = new List<Claim>();
        }
        public static AuthenticationResult Fail(string message)
        {
            return new AuthenticationResult
            {
                Success = false,
                Message = message
            };
        }

        public static AuthenticationResult Succeed(string? message = null)
        {
            return new AuthenticationResult
            {
                Success = true,
                Message = message!
            };
        }



        public static AuthenticationResult PasswordResetToken(string userId, string resetCode, string name)
        {
            return new AuthenticationResult
            {
                Success = true,
                UserInfo = new UserInfo { UserId = userId, Name = name },
                Tokens = new Tokens { PasswordResetToken = resetCode }
            };
        }

        public static AuthenticationResult EmailVerification(string emailVerificationToken)
        {
            return new AuthenticationResult
            {
                Success = true,
                Tokens = new Tokens { EmailVerificationToken = emailVerificationToken }
            };
        }

        public static AuthenticationResult LoginResult(AccessToken accessToken, string userId, string email, string role, object infoObject)
        {
            return new AuthenticationResult
            {
                Success = true,
                Tokens = new Tokens { AccessToken = accessToken },
                UserInfo = new UserInfo { UserId = userId, Email = email, Data = infoObject, Role = role }
            };
        }

        public static AuthenticationResult LoginResult( string name,string email, string role, object infoObject, string token="")
        {
            return new AuthenticationResult
            {
                Success = true,
                Tokens = new Tokens { AccessToken = new AccessToken { Token = token } },
                UserInfo = new UserInfo {  Name = name,Email = email, Data = infoObject, Role = role }
            };
        }

        public static AuthenticationResult ClaimResult(CoursewiseUser user, List<Claim> claims)
        {
            return new AuthenticationResult
            {
                Success = true,
                CoursewiseUser = user,
                UserClaims = claims
            };
        }
        public static AuthenticationResult ClaimResult(string message, CoursewiseUser user, List<Claim> claims)
        {
            return new AuthenticationResult
            {
                Success = true,
                CoursewiseUser = user,
                UserClaims = claims,
                Message = message
            };
        }
    }
}
