
namespace Coursewise.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public string? Desription { get; set; }
        public double Price { get; set; }
        public string PriceString => $"£{Price.ToString("N")}";
        public string Url { get; set; }
        public string? ProviderName { get; set; }
        public double? ProviderPrice { get; set; }
        public bool IsVisible { get; set; }        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<CourseLevel> CourseLevels { get; set; }
    }
}
