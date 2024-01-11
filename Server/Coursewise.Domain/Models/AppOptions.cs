using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class AppOptions
    {
        public string APIVersion { get; set; }
        public string AppUrl { get; set; }
        public string ApiUrl { get; set; }
        public string BoardwiseWPUrl { get; set; }
        public Emails Emails { get; set; }

    }
    public class Emails
    {
        public string ApplyJob { get; set; }
        public string ApplyJobAdmin { get; set; }
        public string ToApplyJobAdmin { get; set; }
        public string NoReply { get; set; }
    }
}
