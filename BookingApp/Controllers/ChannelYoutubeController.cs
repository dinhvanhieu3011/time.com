using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using BookingApp.DB.Classes;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using BookingApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class ChannelYoutubeController : Controller
    {
        public ChannelYoutubeController()
        {

        }

        public IActionResult Index()
        {
            using var db = new AppDbContext();
            var list = db.ChannelYoutubes.ToList();
            List<ChannelYoutubeModel> lst = new List<ChannelYoutubeModel>();

            if (list.Count > 0) {
                foreach (var item in list)
                {
                    var language = db.CookaAccounts.Where(x => x.Id == item.CookaAccountId).Any() ?
                        db.CookaAccounts.Where(x => x.Id == item.CookaAccountId).FirstOrDefault()?.Language : "";
                    var username = db.Users.Where(x => x.UserId == item.UserId).Any() ?
                        db.Users.Where(x => x.UserId == item.UserId).FirstOrDefault()?.Username : "";
                    var category = db.Categorys.Where(x => x.Id == item.CategoryId).Any() ?
                     db.Categorys.Where(x => x.Id == item.CategoryId).FirstOrDefault()?.Name : "";
                    lst.Add(new ChannelYoutubeModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Link = item.Link,
                        Language = language,
                        UserName = username,
                        Category = category,
                    });

                }
            }
            return View(lst);

        }
        #region create
        public IActionResult Create()
        {
            using var db = new AppDbContext();
            var users = db.Users.ToList();
            var cookaccs = db.CookaAccounts.ToList();
            var category = db.Categorys.ToList();
            ViewBag.UserId = new SelectList(users, "UserId", "Username");
            ViewBag.CategoryId = new SelectList(category, "Id", "Name");
            ViewBag.CookaAccountId = new SelectList(cookaccs, "Id", "Language");

            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string Name, string Link, string Category, int CookaAccountId, int UserId, int CategoryId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "ChannelYoutube");
            }

            switch (Insert(Name, Link, Category, CookaAccountId, UserId, CategoryId))
            {
                case true:
                    return RedirectToAction("Index", "ChannelYoutube", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "ChannelYoutube", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "ChannelYoutube", new { error = "error" });
            }
        }
        public bool? Insert(string Name, string Link, string Category, int CookaAccountId, int UserId, int CategoryId)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.ChannelYoutubes.Any(x => x.Link == Link) 
                    && !string.IsNullOrEmpty(Name) 
                    && !string.IsNullOrEmpty(Category)
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
                var cookaccs = db.CookaAccounts.ToList();
                var category = db.Categorys.ToList();

                ViewBag.UserId = new SelectList(users, "UserId", "Username", book.UserId);
                ViewBag.CategoryId = new SelectList(category, "Id", "Name", book.CategoryId);

                ViewBag.CookaAccountId = new SelectList(cookaccs, "Id", "Language",book.CookaAccountId);
                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "ChannelYoutube", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string Name, string Link, string Category, int CookaAccountId, int UserId, int CategoryId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                if (id > 0 &&  !string.IsNullOrEmpty(Name)
                    && !string.IsNullOrEmpty(Category)
                    )
                {

                    UpdateData(id, Name, Link, Category, CookaAccountId, UserId, CategoryId, db);
                    return RedirectToAction("Update", "ChannelYoutube", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "ChannelYoutube", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "ChannelYoutube", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private static void UpdateData(int id, string Name, string Link, string Category, int CookaAccountId, int UserId, int CategoryId, AppDbContext db)
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
                    return RedirectToAction("Index", "ChannelYoutube");
                }

                using var db = new AppDbContext();

                db.Remove(new ChannelYoutubes() { Id = id });
                db.SaveChanges();
                return RedirectToAction("Index", "ChannelYoutube", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "ChannelYoutube", new { error = "error" });
            }
        }
    }
}
