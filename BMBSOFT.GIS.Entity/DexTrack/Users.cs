using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASE.Entity.DexTrack
{
    [Table("users", Schema = "cms")]
    public class Users : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public DateTime Registered { get; set; }
        public string TeleToken { get; set; }
        public string ChatId { get; set; }
    }
}
