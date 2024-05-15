using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using BookingApp.DB.Classes;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using BookingApp.Service;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingApp.Controllers
{
    public class FileController : Controller
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public FileController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(int id)
        {
            using var db = new AppDbContext();
            var channel = db.ChannelYoutubes.Where(x=>x.Id == id).FirstOrDefault();
            var list = db.Books.Where(x=>x.PublicationYear == id).ToList();
            ViewBag.channel = channel.Name;
            return View(list);
        }
        #region create
        public IActionResult Create(int id)
        {
            var db = new AppDbContext();
            var book = db.Books.Where(x => x.BookId == id).FirstOrDefault();

            return View(book);
        }
        #endregion

        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "File");
                }

                using var db = new AppDbContext();

                db.Remove(new Files() { Id = id });
                db.SaveChanges();
                return RedirectToAction("Index", "File", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "File", new { error = "error" });
            }
        }
        public FileResult Download(int id)
        {
            using var db = new AppDbContext();
            var item = db.Files.Where(x=>x.Id == id).FirstOrDefault();
            item.GetedDate = DateTime.Now;
            item.Status = "Đã lấy";
            db.Files.Update(item);
            db.SaveChanges();
            var dir = AppContext.BaseDirectory;
            var FileVirtualPath = dir+"ketqua\\" + item.FilePath;
            FileVirtualPath = dir+"ketqua\\abc.txt";
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }
        public FileResult DownloadFile(int id)
        {
            using var db = new AppDbContext();
            var item = db.Files.Where(x => x.Id == id).FirstOrDefault();
            item.GetedDate = DateTime.Now;
            item.Status = "Đã lấy";
            db.Files.Update(item);
            db.SaveChanges();
            //Build the File Path.
            var dir = AppContext.BaseDirectory;
            var FileVirtualPath = dir + "ketqua\\" + item.FilePath;
            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(FileVirtualPath);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", item.FilePath);

        }
    }
}
