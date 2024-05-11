using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using BookingApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class CookaAccountController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookaAccountController(IHttpContextAccessor httpContextAccessor, ISchedulerService schedulerService)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            using var db = new AppDbContext();
            var list = db.CookaAccounts.ToList();
            return View(list);
        }
        #region create
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string username, string password, string language, int SleepTime)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "CookaAccount");
            }

            switch (Insert(username, password, language, SleepTime))
            {
                case true:
                    return RedirectToAction("Index", "CookaAccount", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "CookaAccount", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "CookaAccount", new { error = "error" });
            }
        }

        public bool? Insert(string username, string password, string language, int SleepTime)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.CookaAccounts.Any(x => x.Username == username) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    db.Add(new CookaAccounts() 
                    { 
                        Username = username,
                        Password = password,
                        Language = language,
                        SleepTime = SleepTime,
                        IsStop = false
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
            var book = db.CookaAccounts.FirstOrDefault(x => x.Id == id);
            if (book != null)
            {

                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "CookaAccount", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string username, string password, string language)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                if (id > 0 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(language))
                {

                    UpdateData(id, username, password, language, db);
                    return RedirectToAction("Update", "CookaAccount", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "CookaAccount", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "CookaAccount", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private static void UpdateData(int id, string username, string password, string language, AppDbContext db)
        {
            var data = db.CookaAccounts.FirstOrDefault(x => x.Id == id);
            data.Username = username;
            data.Password = password;
            data.Language = language;

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
                    return RedirectToAction("Index", "CookaAccount");
                }

                using var db = new AppDbContext();

                db.Remove(new CookaAccounts() { Id = id });
                db.SaveChanges();
                return RedirectToAction("Index", "CookaAccount", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "CookaAccount", new { error = "error" });
            }
        }
    }
}
