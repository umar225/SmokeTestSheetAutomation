using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Job
    {
        public Guid Id { get; set; }
        [MaxLength(150)]
        public string? Name { get; set; }
        [MaxLength(600)]
        public string? Url { get; set; }
        [MaxLength(150)]
        public string? Company { get; set; }
        [MaxLength(200)]
        public string? CompanyTagLine { get; set; }
        [MaxLength(600)]
        public string? CompanyLink { get; set; }
        public int NoOfRole { get; set; }
        [MaxLength(200)]
        public string? ShortDescription { get; set; }
        [Column(TypeName = "longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci")]
        public string? Description { get; set; }
        [MaxLength(150)]
        public string? Category { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Expire { get; set; }
        public bool IsVisible { get; set; }
        public bool IsClosed { get; set; }
        public bool IsEmailSent { get; set; }
        public IEnumerable<JobTitle> JobTitles { get; set; }
        public IEnumerable<JobLocation> JobLocations { get; set; }
        public IEnumerable<JobSkill> JobSkills { get; set; }
        public IEnumerable<JobIndustry> JobIndustry { get; set; }
        public IEnumerable<JobArtifact> JobArtifact { get; set; }
        public IEnumerable<CustomerJob> CustomerJobs { get; set; }
    }
}
