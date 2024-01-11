

namespace Coursewise.Domain.Entities
{
    public class JobArtifact
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; } = null!;
        public int ArtifactId { get; set; }
        public Artifact Artifact { get; set; } = null!;
    }
}
