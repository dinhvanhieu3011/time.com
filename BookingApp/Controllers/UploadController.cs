using BookingApp.DB.Classes.DB;
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
            public IFormFile file { set; get; }
            public DateTime createdDate { set; get; }
            public string comName { set; get; }
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] VideoDto video)
        {
            using var db = new AppDbContext();
            var computers = db.ChannelYoutubes.ToList();
            var com = computers.Where(x => x.Link.Contains(video.comName)).FirstOrDefault();
            // Split the file path using the backslash ('\') character as the separator
            string[] filePathParts = video.file.FileName.Split('\\');
            // Extract the filename without the extension (assuming the last part is the filename)
            string fileNameWithoutExtension = filePathParts[filePathParts.Length - 1];

            string name = fileNameWithoutExtension.Split(".")[0];
            string fileName = name + "_"+com.Id + ".mp4"; 

            string filePath = Path.Combine(_env.ContentRootPath, "file", fileName); // Or use your preferred storage location
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await video.file.CopyToAsync(stream);
            }
            DateTime from = convert(long.Parse(name.Split("_")[0]));
            DateTime to = convert(long.Parse(name.Split("_")[1]));

            db.Add(new Books() 
            { 
                Name = Path.Combine("file", fileName), 
                Author = video.comName.ToUpper(), 
                PublicationYear = com.Id, 
                Registered = video.createdDate,
                Year = from.Year,
                Month = from.Month,
                Day = from.Day,
                Hours = from.Hour,
                Minute = from.Minute,
                fromDate = from.ToString(),
                toDate = to.ToString(),
            });


            db.SaveChanges();

            return Ok("Files uploaded successfully");
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
