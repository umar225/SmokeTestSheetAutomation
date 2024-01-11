using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class WordpressOptions
    {
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public bool IsSendEmail { get; set; }
    }
}
