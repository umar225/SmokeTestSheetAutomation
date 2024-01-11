using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class ResourceArtifact
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Guid ResourceId { get; set; }
        public Resource Resource { get; set; } = null!;
        public int ArtifactId { get; set; }
        public Artifact Artifact { get; set; } = null!;
        public int? ParentResourceArtifactId { get; set; }
        public ResourceArtifact Parent { get; set; }
        public IEnumerable<ResourceArtifact> Variations { get; set; }
    }
}
