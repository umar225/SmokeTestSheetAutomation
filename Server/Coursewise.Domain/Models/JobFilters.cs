using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class JobFilters
    {
        public List<int> Skills { get; set; }
        public List<int> Industries { get; set; }
        public List<int> Locations { get; set; }
        public string SearchString { get; set; } = "";
        public List<int> Titles { get; set; }
        public string Sort { get; set; }
        public DateTime? LastDate { get; set; }
    }
}
