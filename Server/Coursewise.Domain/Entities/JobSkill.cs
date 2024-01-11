using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class JobSkill
    {
        public int Id { get; set; }
        public Guid JobId { get; set; }
        public string? Other { get; set; }
        public Job Job { get; set; } = null!;
        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
    }
}
