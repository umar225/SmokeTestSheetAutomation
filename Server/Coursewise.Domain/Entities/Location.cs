using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoOfJob { get; set; }
        public IEnumerable<JobLocation> JobLocations { get; set; }
    }
}
