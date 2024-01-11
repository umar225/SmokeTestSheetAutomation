using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class JobSkill
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string? Other { get; set; }
        public Guid JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public Skill Skill { get; set; } = null!;
    }
}
