using Microsoft.AspNetCore.Http;

namespace BASE.Model.Dextrack
{
    public class VideoDto
    {
        public IFormFile Video { set; get; }
        public IFormFile UserAction { set; get; }
        public IFormFile UserSession { set; get; }
        public string token { set; get; }
        public string linkLive { set; get; }
    }
}
