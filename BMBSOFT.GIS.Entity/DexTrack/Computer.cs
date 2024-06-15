using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASE.Entity.DexTrack
{

    [Table("computer", Schema = "cms")]
    public class ChannelYoutubes : BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
    }
}
