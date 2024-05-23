using System;
using System.Linq;
using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BookingApp.Controllers
{
    public class VideoController : Controller
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public VideoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int pageSize = 10;

        public IActionResult Index(int id)
        {
            using var db = new AppDbContext();
            var data = db.Videos.Where(x=>x.ChannelId == id&& x.IsDelete == 0).OrderByDescending(x=>x.Id).ToList();

            return View(data);

        }
        public Videos Get(int id)
        {
            var db = new AppDbContext();

            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
            book.VideoPath = @"/" + book.VideoPath.Replace(@"\", @"/");
            return book;
        }
        public IActionResult Detail(int id)
        {
            var db = new AppDbContext();
  
            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
            var channel = db.ChannelYoutubes.Where(x => x.Id == book.ChannelId).FirstOrDefault();
            var relatedVideo = db.Videos.Where(x => x.ChannelId == channel.Id && x.Year == book.Year 
            && x.Month == book.Month && x.Date == book.Date && x.IsDelete == 0 && x.Id != id)
                .OrderByDescending(x => x.Id).Take(12).ToList();
            ViewBag.relatedVideo = relatedVideo;
            ViewBag.channel = channel.Name;
            ViewBag.filePath = @"/" + book.VideoPath.Replace(@"\",@"/");
            return View(book);
        }

    }
}
