using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class CustomerJob
    {
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; } = string.Empty;
        public DateTime ApplyAt { get; set; }
        public int? ArtifactId { get; set; }
        [ForeignKey("ArtifactId")]
        public Artifact? Artifact { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;
        public Guid JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;
    }
}
