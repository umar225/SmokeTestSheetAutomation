using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class JobTitle
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string? Other { get; set; }
        public Guid JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;
        public int TitleId { get; set; }
        [ForeignKey("TitleId")]
        public Title Title { get; set; } = null!;
    }
}
