using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BASE.CORE.Helper;
using Microsoft.AspNetCore.Identity;

namespace BASE.Entity.IdentityAccess
{
    [Table("AspNetRoleClaims",Schema = Constant.Schema.AUTHENTICATION)]
    public class RoleClaim : IdentityRoleClaim<string>
    {
        [Key]
        public override string RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
