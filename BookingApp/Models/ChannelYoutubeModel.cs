using System.ComponentModel.DataAnnotations;

namespace BookingApp.Models
{
    public class ChannelYoutubeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
        public int CookaAccountId { get; set; }
        public string Language { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Status { set; get; }  
    }

}
