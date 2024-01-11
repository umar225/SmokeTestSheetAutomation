
namespace Coursewise.Domain.Models.Dto
{
    public class BillingDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; }
        public string IntentId { get; set; }
        public string Number { get; set; }
        public string Notes { get; set; }
       
        
    }

    public class Address 
    {
       public string City
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public string Line1
        {
            get;
            set;
        }

         public string Line2
        {
            get;
            set;
        }

        public string PostalCode
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }
    }
}
