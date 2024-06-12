using Microsoft.AspNetCore.Http;

namespace BASE.Model.Dextrack
{
    public class ImageDto
    {
        public IFormFile Image { set; get; }
        public string token { set; get; }

    }
}
