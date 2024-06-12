using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASE.Entity.IdentityAccess
{
    [Table("AspNetUsers", Schema = "authentication")]
    public class User : IdentityUser<string>
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; } = false;
        public string Description { get; set; }
        public string UserType { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
        public virtual ICollection<UserClaim> UserClaims { get; } = new List<UserClaim>();
    }
}
