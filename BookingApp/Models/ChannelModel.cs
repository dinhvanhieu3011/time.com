using System;

namespace BookingApp.Models
{
    public class ChannelModel
    {
        public int ChannelId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int Action { get; set; }
        public string ActionName { get; set; }
        public int VideoCount { get; set; }
        public DateTime LastVideoCreatedDate { get; set; }
        public string LastVideoCreatedDateString { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }

    }
}
