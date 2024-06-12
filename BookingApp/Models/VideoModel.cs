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
}
