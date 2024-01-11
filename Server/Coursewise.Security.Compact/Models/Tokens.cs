using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Models
{
    public class Tokens
    {
        public string PasswordResetToken { get; set; }
        public string EmailVerificationToken { get; set; }
        public AccessToken AccessToken { get; set; }
    }

    public class AccessToken
    {
        public string Token { get; set; }
        public string Expiry { get; set; }
    }
}
