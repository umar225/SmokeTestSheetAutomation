
namespace Coursewise.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? DisplayName { get; set; }


        public IEnumerable<Course> Courses { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDeleted { get; set; }
    }
}
