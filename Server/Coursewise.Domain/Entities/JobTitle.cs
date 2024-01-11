using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class JobTitle
    {
        public int Id { get; set; }
        public string? Other { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; } = null!;
        public int TitleId { get; set; }
        public Title Title { get; set; } = null!;
    }
}
