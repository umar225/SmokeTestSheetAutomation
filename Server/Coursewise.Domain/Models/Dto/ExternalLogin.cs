using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ExternalLoginModel
    {

        public string Provider { get; set; }
        public string AccessToken { get; set; }

    }
    public class LinkedinAccessTokenRequest
    {
        public string grant_type { get; set; }
        public string code { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
    }
    public class ExternalLoginRegisterModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
