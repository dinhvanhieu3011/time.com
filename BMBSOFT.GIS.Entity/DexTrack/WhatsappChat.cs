using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASE.Entity.DexTrack
{
    [Table("whatsapp_chat", Schema = "cms")]
    public class WhatsAppChat : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string ChatId { get; set; }
        public long Timestamp { get; set; }
        public DateTime Time { get; set; }
        public string FromPhoneNumber { get; set; }
        public string FromId { get; set; }
        public string ToPhoneNumber { get; set; }
        public string ToId { get; set; }
        public string Message { get; set; }
    }
}
