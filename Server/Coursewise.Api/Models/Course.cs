using System;
using System.Collections.Generic;

namespace Coursewise.Api.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Desription { get; set; }
        public double Price { get; set; }
        public List<Companay> Companies { get; set; }
    }

    public class Companay
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string PriceString => $"£{Price.ToString("F2")}";
    }
}