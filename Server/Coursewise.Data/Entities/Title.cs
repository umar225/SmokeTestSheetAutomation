using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Title
    {
        [Key]
        public int TitleId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        public int NoOfJob { get; set; }
        public IEnumerable<JobTitle> JobTitles { get; set; }
    }
}
