using Coursewise.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class EmailTemplate
    {
        public int Id { get; set; }
        public EmailType Type { get; set; }
        [Column(TypeName = "longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci")]
        public string Body { get; set; }
    }
}
