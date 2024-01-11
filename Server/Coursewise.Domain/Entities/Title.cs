using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class Title
    {
        public int TitleId { get; set; }
        public string Name { get; set; }
        public int NoOfJob { get; set; }
        public IEnumerable<JobTitle> JobTitles { get; set; }
    }
}
