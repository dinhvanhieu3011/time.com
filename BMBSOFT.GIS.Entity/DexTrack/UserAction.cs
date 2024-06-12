using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASE.Entity.DexTrack
{
    [Table("user_action", Schema = "cms")]
    public class UserAction
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public DateTime Time { get; set; } = DateTime.Now.ToLocalTime();
        public string Windows { get; set; }
        public string UserName { get; set; }
        public string Keys { get; set; }
        public string VideoId { get; set; }
    }
}
