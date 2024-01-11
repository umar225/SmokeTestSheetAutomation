using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class Job
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Company { get; set; }
        public string? CompanyTagLine { get; set; }
        public string? CompanyLink { get; set; }
        public int NoOfRole { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
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
