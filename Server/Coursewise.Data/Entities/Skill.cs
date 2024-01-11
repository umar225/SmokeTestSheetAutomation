using System.ComponentModel.DataAnnotations;

namespace Coursewise.Data.Entities
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        public int NoOfJob { get; set; }
        public IEnumerable<JobSkill> JobSkills { get; set; }
    }
}
