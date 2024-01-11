
namespace Coursewise.Domain.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoOfJob { get; set; }
        public IEnumerable<JobSkill> JobSkills { get; set; }
    }
}
