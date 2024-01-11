using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication.Models
{
    public class Email
    {
        public string To { get; set; }
        public List<string> ToAddress { get; set; }

        public string FromName { get; set; }

        public string FromEmail { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }


        public int Priority { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }


        public string ReplyTo { get; set; }


        public ICollection<EmailAttachment> Attachments { get; set; }
    }
}
