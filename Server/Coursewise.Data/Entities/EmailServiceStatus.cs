using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class ServiceStatus
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsRunning { get; set; } = false;
    }
}
