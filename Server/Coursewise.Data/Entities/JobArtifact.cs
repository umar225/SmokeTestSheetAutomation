using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class JobArtifact
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        public Guid JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;
        public int ArtifactId { get; set; }
        [ForeignKey("ArtifactId")]
        public Artifact Artifact { get; set; } = null!;
    }
}
