using System;
using System.Linq;
using BookingApp.DB.Classes;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class UserController : Controller
    {
        public UserController()
        {

        }

        public IActionResult Index()
        {
            using var db = new AppDbContext();
            var list = db.Users.ToList();
            return View(list);
        }
        #region create
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string username, string password, string teleToken, string chatId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "User");
            }

            switch (Insert(username, password, teleToken, chatId))
            {
                case true:
                    return RedirectToAction("Index", "User", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "User", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "User", new { error = "error" });
            }
        }

        public bool? Insert(string username, string password, string teleToken, string chatId)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.Users.Any(x => x.Username == username) 
                    && !string.IsNullOrEmpty(username) 
                    && !string.IsNullOrEmpty(password)
                    && !string.IsNullOrEmpty(teleToken)
                    && !string.IsNullOrEmpty(chatId)
                    )
                {
                    db.Add(new Users()
                    {
                        Username = username,
                        Password = password,
                        Email = username+ "_email@gmail.com",
                        Role = 1,
                        Registered = DateTime.Now,
                        TeleToken = teleToken,
                        ChatId = chatId,
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
            var book = db.Users.FirstOrDefault(x => x.UserId == id);
            if (book != null)
            {

                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "User", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string username, string password, string teleToken, string chatId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                if (id > 0 && !string.IsNullOrEmpty(username) 
                    && !string.IsNullOrEmpty(password)
                    && !string.IsNullOrEmpty(password)
                    && !string.IsNullOrEmpty(teleToken)
                    && !string.IsNullOrEmpty(chatId)
                    )
                {

                    UpdateData(id, username, password, teleToken, chatId, db);
                    return RedirectToAction("Update", "User", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "User", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private static void UpdateData(int id, string username, string password, string teleToken, string chatId,  AppDbContext db)
        {
            var data = db.Users.FirstOrDefault(x => x.UserId == id);
            data.Username = username;
            data.Password = password;
            data.TeleToken = teleToken;
            data.ChatId = chatId;

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
                    return RedirectToAction("Index", "User");
                }

                using var db = new AppDbContext();

                db.Remove(new Users() { UserId = id });
                db.SaveChanges();
                return RedirectToAction("Index", "User", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "User", new { error = "error" });
            }
        }
    }
}
