using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            string filePath = Path.Combine(_env.ContentRootPath, "file", video.file.FileName); // Or use your preferred storage location
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await video.file.CopyToAsync(stream);
            }
            using var db = new AppDbContext();
            var computers = db.ChannelYoutubes.ToList();
            var com = computers.Where(x=>x.Name == video.comName).FirstOrDefault();

            db.Add(new Books() 
            { 
                Name = Path.Combine("file", video.file.FileName), 
                Author = video.comName.ToUpper(), 
                PublicationYear = com.Id, 
                Registered = video.createdDate 
            });


            db.SaveChanges();

            return Ok("Files uploaded successfully");
        }
    }
}
