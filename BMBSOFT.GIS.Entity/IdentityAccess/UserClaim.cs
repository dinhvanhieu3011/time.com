using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BASE.Entity.IdentityAccess
{
    [Table("AspNetUserClaims", Schema = "authentication")]
    public class UserClaim : IdentityUserClaim<string>
    {
        [Key]
        public override string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
