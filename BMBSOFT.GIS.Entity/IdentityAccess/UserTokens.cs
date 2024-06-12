using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BASE.Entity.IdentityAccess
{
    [Table("AspNetUserTokens", Schema = "authentication")]
    public class UserTokens : IdentityUserToken<string>
    {
        public DateTime? ExpiredTime { get; set; }
        public string DeviceId { get; set; }
    }
}
