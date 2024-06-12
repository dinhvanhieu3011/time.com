using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BASE.Entity.IdentityAccess
{
    [Table("AspNetRoles",Schema = "authentication")]
    public class Role : IdentityRole<string>
    {
        public Role() { }

        public Role(string roleName)
            : base(roleName)
        {
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
        public virtual ICollection<RoleClaim> RoleClaims { get; } = new List<RoleClaim>();
        public ICollection<SecurityMatrix.SecurityMatrix> SecurityMatrices { set; get; } = new List<SecurityMatrix.SecurityMatrix>();
    }
}
