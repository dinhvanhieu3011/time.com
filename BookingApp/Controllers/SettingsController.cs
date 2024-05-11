using System.Linq;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class SettingsController : Controller
    {
        public const string PASS_KEY = "!emJ(?w)Sx_5S-3L";

        public SettingsController()
        {

        }

        public IActionResult Index()
        {
            using var db = new AppDbContext();

            var settings = db.Settings.FirstOrDefault();

            return View(new SettingsModel
            {
                Port = settings.Port.ToString(),
                MaxTime = settings.MaxTime,
                Email = settings.Email,
                Host = settings.MailHost
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string port, string max, string email, string host, string password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                if  (string.IsNullOrEmpty(host))
                {
                    return RedirectToAction("Index", "Settings", new { error = "không được để rỗng" });
                }

                using var db = new AppDbContext();

                var settings = db.Settings.FirstOrDefault();



                settings.MailHost = host;
                settings.Email = email;

                db.Update(settings);
                db.SaveChanges();

                return RedirectToAction("Index", "Settings", new { msg = "updated" });
            }
            catch
            {
                return RedirectToAction("Index", "Settings", new { error = "error" });
            }
        }
    }
}
