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
    public class CategoryController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryController(IHttpContextAccessor httpContextAccessor, ISchedulerService schedulerService)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            using var db = new AppDbContext();
            var list = db.Categorys.ToList();
            return View(list);
        }
        #region create
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string name)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Category");
            }

            switch (Insert(name))
            {
                case true:
                    return RedirectToAction("Index", "Category", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "Category", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "Category", new { error = "error" });
            }
        }

        public bool? Insert(string name)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.Categorys.Any(x => x.Name == name) && !string.IsNullOrEmpty(name))
                {
                    db.Add(new Category() 
                    { 
                        Name = name,
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
            var book = db.Categorys.FirstOrDefault(x => x.Id == id);
            if (book != null)
            {

                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "Category", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string name)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                if (id > 0 && !string.IsNullOrEmpty(name))
                {

                    UpdateData(id, name,  db);
                    return RedirectToAction("Update", "Category", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "Category", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Category", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private static void UpdateData(int id, string name, AppDbContext db)
        {
            var data = db.Categorys.FirstOrDefault(x => x.Id == id);
            data.Name = name;
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
                    return RedirectToAction("Index", "Category");
                }

                using var db = new AppDbContext();

                db.Remove(new Category() { Id = id });
                db.SaveChanges();
                return RedirectToAction("Index", "Category", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Category", new { error = "error" });
            }
        }
    }
}
