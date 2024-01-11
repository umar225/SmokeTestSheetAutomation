using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class OneToOneFilters
    {
        public string Email { get; set; } = "";
        public string userType { get; set; } = "all";
        public int Page { get; set; } = 1;
    }
    public class SubscribeOneToOne
    {
        public Guid Id { get; set; } 
        public bool Subscribe { get; set; } 
    }
}
