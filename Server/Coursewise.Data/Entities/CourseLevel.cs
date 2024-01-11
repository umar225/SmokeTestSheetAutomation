using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class CourseLevel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;
        public Guid LevelId { get; set; }
        [ForeignKey("LevelId")]
        public Level Level { get; set; } = null!;
    }
}
