using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class MembershipDto
    {
        public string ProductId { get; set; }
        public string PriceId { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set;  }
        public bool isSubscribed { get; set; } = false;
        public string Expiry { get; set; }

    }
}
