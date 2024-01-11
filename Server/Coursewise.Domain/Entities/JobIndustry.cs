using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class JobIndustry
    {
        public int Id { get; set; }
        public string? Other { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; } = null!;
        public int IndustryId { get; set; }
        public Industry Industry { get; set; } = null!;
    }
}
