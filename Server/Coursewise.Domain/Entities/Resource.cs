using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class Resource
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Preview { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Length { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsReady { get; set; }
        public int NoOfClaps { get; set; }
        public int ResourceType { get; set; } =(int) Common.Models.ResourceType.IMAGE_RESOURCE;
        public string? VideoLink { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ResourceArtifact> ResourceArtifacts { get; set; }
    }
}
