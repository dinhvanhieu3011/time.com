using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASE.Entity.DexTrack
{
    [Table("videos", Schema = "cms")]
    public class Videos : BaseEntity
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string VideoPath { get; set; }
        public string Keylog { get; set; }
        public string Apps { get; set; }
        public int ChannelId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int IsDelete { get; set; }
        public int IsMerge { get; set; }
        public string Thumbnail { get; set; }
    }
}
