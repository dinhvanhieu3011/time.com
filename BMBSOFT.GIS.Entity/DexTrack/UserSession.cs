using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASE.Entity.DexTrack
{
    [Table("user_session", Schema = "cms")]
    public class UserSession
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Windows { get; set; }
        public string VideoId { get; set; }
    }
}
