using System.Web;
using Coursewise.Common.Utilities;
namespace Coursewise.Domain.Models
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Desription { get; set; }
        public string Level { get; set; }
        public double Price { get; set; }
        public string PriceString => $"£{Price.ToString("N")}";
        public string Urls { get; set; }
        public string ProviderName { get; set; }
        public double? ProviderPrice { get; set; }
        public string ProviderPriceString => ProviderPrice.HasValue? $"£{ProviderPrice.Value.ToString("N")}":"";        
    }
}