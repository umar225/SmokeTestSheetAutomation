using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class ResourceArtifact
    {
        public int Id { get; set; }
        public Guid ResourceId { get; set; }
        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; } = null!;
        public int ArtifactId { get; set; }
        [ForeignKey("ArtifactId")]
        public Artifact Artifact { get; set; } = null!;
        [MaxLength(100)]
        public string Type { get; set; }
        public int? ParentResourceArtifactId { get; set; }
        [ForeignKey("ParentResourceArtifactId")]
        public ResourceArtifact Parent { get; set; }
        public IEnumerable<ResourceArtifact> Variations { get; set; }

    }
}
