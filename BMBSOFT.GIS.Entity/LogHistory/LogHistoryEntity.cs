using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BASE.CORE.Helper;

namespace BASE.Entity.LogHistory
{
    [Table("log_history", Schema = Constant.Schema.CMS)]
    public class LogHistoryEntity : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("action")]
        public int Action { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}