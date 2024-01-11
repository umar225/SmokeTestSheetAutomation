using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class JobBasicDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? PreviewText { get; set; }
        public string? Company { get; set; }
        public int NoOfRole { get; set; }
        public string? Category { get; set; }
        public string? CompanyLogo { get; set; }
        public DateTime Created { get; set; }
        public bool IsAlreadyApplied { get; set; } = false;

    }

    public class JobDetaiDto: JobBasicDto
    {
        public string? CompanyLink { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<string> Industries { get; set; }
        public IEnumerable<string> Titles { get; set; }
    }
}
