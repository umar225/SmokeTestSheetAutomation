using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class Order
    {
        public string? IntentId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
    public class SubscriptionInformation
    {
        public string ProductId { get; set; }
        public string PriceId { get; set; }
    }
    public class SubscriptionCreate
    {
        public string Email { get; set; }
        public string PriceId { get; set; }
       public string PaymentMethodId { get; set; }
    }
}
