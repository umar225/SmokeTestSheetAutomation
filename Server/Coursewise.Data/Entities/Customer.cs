using Coursewise.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        [MaxLength(127)]
        public string? Email { get; set; }
        [MaxLength(127)]
        public string? FirstName { get; set; }
        [MaxLength(127)]
        public string? LastName { get; set; }
        public int? WordpressBoardwiseUserId { get; set; }
        public string? UserId { get; set; }
        public bool isMember { get; set; }
        public bool AutoRenewMembership { get; set; } = false;
        public bool IsOneToOneMember { get; set; } = false;
        public DateTime OneToOneExpiryDate { get; set; } = DateTime.Now.AddDays(365);

        public DateTime MembershipExpiryDate { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        public CoursewiseUser CoursewiseUser { get; set; }
    }
}
