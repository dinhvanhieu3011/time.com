using BASE.Entity.DexTrack;
using System.Collections.Generic;

namespace BookingApp.Models
{
    public class VideoModel
    {
        public Videos Video { get; set; }
        public List<UserAction>  userActions { get; set; }
        public List<UserSession>  userSessions { get; set; }
    }
    public class VideoGroupHour
    {
        public string Video { get; set; }
        public string Thumbnail { get; set; }
        public string Hour { get; set; }
        public string Date { get; set; }
    }
}
