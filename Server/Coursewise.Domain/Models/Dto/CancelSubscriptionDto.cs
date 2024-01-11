using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class CancelSubscriptionDto
    {
        public int SubscriptionType { get; set; }
        public bool isCancel { get; set; }
    }
}
