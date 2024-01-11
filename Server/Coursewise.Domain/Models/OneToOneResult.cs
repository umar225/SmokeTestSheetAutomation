using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class OneToOneResult
    {
        public Pagination Pagination { get; set; }=new Pagination();
        public List<UserInformartion> Users { get; set; }=new List<UserInformartion>() { }; 
    }
    public class Pagination {
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public int CurrentPage { get; set; }
      
    }
    public class UserInformartion {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public bool IsOneToOneMember { get; set; } = false;
        public DateTime OneToOneExpiryDate { get; set; }
    }
}
