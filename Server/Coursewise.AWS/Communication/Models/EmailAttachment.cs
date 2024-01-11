using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication.Models
{
    public class EmailAttachment
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public long Id { get; set; }
        public byte[] Data { get; set; }
        public Stream DataStream { get; set; }
    }
}
