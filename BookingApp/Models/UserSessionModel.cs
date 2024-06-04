using System;

namespace BookingApp.Models
{
    public class UserSessionModel
    {
        public int Id { get; set; }
        public int StartTime { get; set; }
        public DateTime Start { get; set; }
        public int EndTime { get; set; }
        public DateTime End { get; set; }
        public string UseTime { get; set; }
        public string Windows { get; set; }
        public int VideoId { get; set; }
        public string ComputerName { get; set; }
    }
}
