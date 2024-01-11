using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Common.Models
{
    public class CoursewiseUser : IdentityUser<string>
    {
        public string Name { get; set; }
        public DateTime? PasswordResetAt { get; set; }
        public bool JobNotification { get; set; } = true;
        public bool ResourceNotification { get; set; } = true;
        public int FreeResourcesCount { get; set; } = 0;
        public string? StripeCustomerId { get; set; }
    }
}
