
namespace Coursewise.Domain.Entities
{
    public class Level
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CourseLevel> CourseLevels { get; set; }
    }
}
