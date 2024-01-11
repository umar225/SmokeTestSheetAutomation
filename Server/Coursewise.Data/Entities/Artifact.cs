using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Artifact
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string StorageLocation { get; set; }
        [StringLength(30)]
        public string FileType { get; set; }
        [StringLength(512)]
        public string Url { get; set; }
    }
}
