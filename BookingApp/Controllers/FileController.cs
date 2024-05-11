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

        public IActionResult Index()
        {

            var user = new AppDbContext().Users.FirstOrDefault(x => x.Username ==
            _httpContextAccessor.HttpContext.Session.GetString("user"));
            using var db = new AppDbContext();
            var listChannel = db.ChannelYoutubes.Where(x => x.UserId == user.UserId).Select(x=>x.Link).ToList();
            if(user.Role == 0)
            {
                var list = db.Files.ToList();
                return View(list);

            }
            else
            {
                var list = db.Files.Where(x => listChannel.Contains(x.ChannelYoutubeName)).ToList();
                return View(list);
            }
        }
        #region create
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region update
        public IActionResult Update(int id)
        {
            using var db = new AppDbContext();
            var book = db.Files.FirstOrDefault(x => x.Id == id);

            if (book != null)
            {

                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "File", new { error = "wrongData" });
            }
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
        //public IActionResult Download(int id)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return RedirectToAction("Index", "File");
        //        }

        //        using var db = new AppDbContext();

        //        db.Remove(new Files() { Id = id }); 
        //        db.SaveChanges();
        //        return RedirectToAction("Index", "File", new { msg = "Deleted" });

        //    }
        //    catch
        //    {
        //        return RedirectToAction("Index", "File", new { error = "error" });
        //    }
        //}
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
        public FileResult DownloadFileEng(int id)
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

            //FileVirtualPath = dir + "ketqua\\abc.txt";
            Helper.CallPython(FileVirtualPath);
            //Read the File data into Byte Array.
            string batchFileDirectory = System.IO.Directory.GetCurrentDirectory(); // Change this to the desired directory
            string newFilePath = Path.Combine(batchFileDirectory, "temp.txt");
            byte[] bytes = System.IO.File.ReadAllBytes(newFilePath + ".eng.txt");

            //Send the File to Download.
            return File(bytes, "application/octet-stream", item.FilePath);

        }
    }
}
