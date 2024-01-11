using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(300)]
        public string Name { get; set; }
        [Column(TypeName = "longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci")]
        public string? Desription { get; set; }
        public double Price { get; set; }
        [MaxLength(300)]
        public string Url { get; set; }
        [MaxLength(300)]
        public string? ProviderName { get; set; }
        public double? ProviderPrice { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
        public IEnumerable<CourseLevel> CourseLevels { get; set; }
    }
}
