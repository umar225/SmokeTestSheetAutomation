using Coursewise.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class EmailTemplate
    {
        public int Id { get; set; }
        public EmailType Type { get; set; }
        public string Body { get; set; }
    }
}
