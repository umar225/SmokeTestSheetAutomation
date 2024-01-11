using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Resource
    {
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Preview { get; set; } = string.Empty;
        [Column(TypeName = "longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci")]
        public string Description { get; set; } = string.Empty;
        public int Length { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsEmailSent { get; set; }
        public bool isDeleted { get; set; } = false;
        public int ResourceType { get; set; } =(int)Common.Models.ResourceType.IMAGE_RESOURCE;
        public string? VideoLink { get; set; }
        public bool IsReady { get; set; }
        public int NoOfClaps { get; set; }
        [MaxLength(300)]
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ResourceArtifact> ResourceArtifacts { get; set; }
    }
}
