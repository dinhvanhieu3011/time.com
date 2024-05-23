using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using BookingApp.DB.Classes;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using BookingApp.Service;
using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index(int id, string txtSearch, int? page)
        {
            using var db = new AppDbContext();
            var channel = db.ChannelYoutubes.Where(x=>x.Id == id).FirstOrDefault();
            var data = db.Videos.Where(x=>x.ChannelId == id);

            ViewBag.channel = channel.Name;

            if (page != 0)
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            ViewBag.channelId = id;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            ViewBag.videos = data.OrderByDescending(x => x.Id).Skip(start).Take(pageSize);
            return View();

        }
        public IActionResult Detail(int id)
        {
            var db = new AppDbContext();
            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
           
            ViewBag.VideoPath = @"/" + book.VideoPath.Replace(@"\",@"/");
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Video");
                }

                using var db = new AppDbContext();

                db.Remove(new Videos() { Id = id });
                db.SaveChanges();
                return RedirectToAction("Index", "Video", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Video", new { error = "error" });
            }
        }
    }
}
