using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASE.Entity.IdentityAccess
{
    [Table("AspNetUserRoles",Schema = "authentication")]
    public class UserRole : IdentityUserRole<string>
    {
        public virtual Role Role { get; set; }

        public virtual User User { get; set; }

    }
}
