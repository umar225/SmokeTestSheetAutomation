using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class CustomerJob
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ApplyAt { get; set; }
        public int? ArtifactId { get; set; }
        public Artifact? Artifact { get; set; } = null!;
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public Guid JobId { get; set; }
        public Job Job { get; set; } = null!;
    }
}
