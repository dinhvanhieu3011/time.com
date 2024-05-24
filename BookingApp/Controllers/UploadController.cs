using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static BookingApp.Controllers.VideoController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        public class JsonResponse
        {
            public object Data { get; set; }

            public string Message { get; set; }

            public bool Success { get; set; }

            public string Pager { get; set; }

            public string Id { get; set; }
        }
        private readonly JsonResponse response = new JsonResponse();
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
        public JsonResponse getList(string Ngay, int id,  int pageIndex, int pageSize)
        {
            try
            {
                var totalRow = 0;
                List<Videos> data = new List<Videos>();
                CultureInfo provider = CultureInfo.CurrentCulture;
                if (string.IsNullOrEmpty(Ngay))
                {
                    using var db = new AppDbContext();
                    data = db.Videos.Where(x=>(x.ChannelId == id || id == 0) && x.IsDelete == 0).ToList();
                }
                else
                {
                    DateTime dateTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider);
                    using var db = new AppDbContext();
                    data = db.Videos.Where(x=> (x.ChannelId == id || id == 0) && x.Start.Date == dateTime && x.IsDelete == 0).ToList();
                }

                totalRow = data.Count();
                this.response.Data = data.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                this.response.Success = true;
                this.response.Pager = totalRow.ToString();
                return this.response;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.Message = ex.Message;
                return this.response;
            }
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
                DateTime from = convert(long.Parse(name.Split("_")[0]));
                DateTime to = convert(long.Parse(name.Split("_")[1]));
                string folder = from.Date.ToString("ddMMyyyy");
                if (!Directory.Exists(Path.Combine(_env.ContentRootPath, folder)))
                {
                    // Nếu không tồn tại, tạo thư mục mới
                    Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, folder));
                }
                if (!Directory.Exists(Path.Combine(_env.ContentRootPath, folder, com.Id.ToString())))
                {
                    // Nếu không tồn tại, tạo thư mục mới
                    Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, folder, com.Id.ToString()));
                }
                string fPath = Path.Combine(_env.ContentRootPath, "file", folder, com.Id.ToString(), VideoName); // Or use your preferred storage location
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    await video.Video.CopyToAsync(stream);

                }

                
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
