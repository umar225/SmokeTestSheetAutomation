
namespace Coursewise.Domain.Entities
{
    public class CourseLevel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid LevelId { get; set; }
        public Level Level { get; set; } = null!;
    }
}
