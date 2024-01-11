using Coursewise.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }


        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? WordpressBoardwiseUserId { get; set; }
        public string UserId { get; set; }
        public bool isMember { get; set; }
        public bool IsOneToOneMember { get; set; }
        public DateTime OneToOneExpiryDate { get; set; } = DateTime.Now.AddYears(1);
        public DateTime MembershipExpiryDate { get; set; }=DateTime.Now;
        public CoursewiseUser CoursewiseUser { get; set; }
    }
}
