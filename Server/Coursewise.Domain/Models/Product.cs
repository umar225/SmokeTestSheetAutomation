using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public string? Provider { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
