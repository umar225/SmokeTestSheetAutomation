using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class PricingPageDto
    {
        public bool IsFreeMember { get; set; } = false;
        public bool IsQuartelylyMember { get; set; } = false;
        public bool IsYearlyMember { get; set; } = false;
        public bool IsOneToOneMember { get; set; } = false;
        public DateTime OneToOneExpiryDate { get; set; } = DateTime.Now.AddDays(365);
        public DateTime? FreeExpiry { get; set; }
        public DateTime? QuarterlyExpiry { get; set; }
        public DateTime? YearlyExpiry { get; set; }
        public bool IsMembershipActive { get; set; } = false;



    }
    public enum SubscriptionType
    {
        Quarterly = 1,
        Yearly=2,
    }
}
