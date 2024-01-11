using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string? DisplayName { get; set; }

        
        public IEnumerable<Course> Courses { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDeleted { get; set; }
    }
}
