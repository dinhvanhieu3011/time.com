using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using BookingApp.DB.Classes;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using BookingApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class ComputerController : Controller
    {
        private readonly ILogger<ComputerController> _logger;

        public ComputerController(ILogger<ComputerController> logger)
        {
            _logger = logger;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Index()
        {
            using var db = new AppDbContext();
            var list = db.ChannelYoutubes.ToList();
            List<ChannelYoutubeModel> lst = new List<ChannelYoutubeModel>();

            if (list.Count > 0) {
                foreach (var item in list)
                {

                    lst.Add(new ChannelYoutubeModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Link = item.Link,
                        Language = "",
                        UserId = item.UserId,
                        Category = item.Category,
                        Status = item.CategoryId == 0 ? "Mất kết nối" : "Đang online"
                    });

                }
            }
            return View(lst);

        }
        #region create
        public IActionResult Create()
        {
            using var db = new AppDbContext();

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string Name, string Link, string UserId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Computer");
            }
            Link = Guid.NewGuid().ToString();
            switch (Insert(Name, Link, "", 0, UserId, 0))
            {
                case true:
                    _logger.LogInformation("Tạo Máy " + Name + " thành công!");
                    return RedirectToAction("Index", "Computer", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "Computer", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "Computer", new { error = "error" });
            }
        }
        public bool? Insert(string Name, string Link, string Category, int CookaAccountId, string UserId, int CategoryId)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.ChannelYoutubes.Any(x => x.Link == Link) 
                    && !string.IsNullOrEmpty(Name) 
                    )
                {
                    db.Add(new ChannelYoutubes()
                    {
                        Name = Name,
                        Link = Link,
                        Category = Category,
                        CookaAccountId = CookaAccountId,
                        UserId = UserId,
                        CategoryId = CategoryId,
                    });

                    db.SaveChanges();


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region update
        public IActionResult Update(int id)
        {
            using var db = new AppDbContext();
            var book = db.ChannelYoutubes.FirstOrDefault(x => x.Id == id);

            if (book != null)
            {
                var users = db.Users.ToList();

                _logger.LogInformation("Chỉnh sửa Máy " + book.Id + " thành công!");
                ViewBag.UserId = new SelectList(users, "UserId", "Username", book.UserId);
                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "Computer", new { error = "wrongData" });
            }
        }
        public IActionResult Live(int id)
        {
            using var db = new AppDbContext();
            var book = db.ChannelYoutubes.FirstOrDefault(x => x.Id == id);

            if (book != null)
            {
                ViewBag.Ip = @"/live/" + book.Link+".jpg";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Computer", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string Name, string Link, string UserId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                if (id > 0 &&  !string.IsNullOrEmpty(Name)   
                    )
                {

                    UpdateData(id, Name, Link, "", 0, UserId, 0, db);
                    return RedirectToAction("Update", "Computer", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "Computer", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Computer", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private static void UpdateData(int id, string Name, string Link, string Category, int CookaAccountId, string UserId, int CategoryId, AppDbContext db)
        {
            var data = db.ChannelYoutubes.FirstOrDefault(x => x.Id == id);
            data.Name = Name;
            data.Link = Link;
            data.Category = Category;
            data.CookaAccountId = CookaAccountId;
            data.UserId = UserId;
            data.CategoryId = CategoryId;

            db.Update(data);
            db.SaveChanges();
        }
        #endregion
        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Computer");
                }

                using var db = new AppDbContext();

                db.Remove(new ChannelYoutubes() { Id = id });
                db.SaveChanges();
                _logger.LogInformation("Xoá Máy " + id + " thành công!");

                return RedirectToAction("Index", "Computer", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Computer", new { error = "error" });
            }
        }
    }
}
