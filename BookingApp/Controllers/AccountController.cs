
using System.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace AccountingApp.Controllers
{
    public class AccountController : Controller
    {
        public string FileNameOnServer { get; set; }
        public long FileContentLength { get; set; }
        public string FileContentType { get; set; }
        public AccountController()
        {
            FileNameOnServer = string.Empty;
            FileContentLength = 0;
            FileContentType = string.Empty;
        }
        [HttpGet]
        [AllowAnonymous]
        public string Get()
        {
            try
            {
                using var db = new AppDbContext();
                Accounts acc = db.Accounts.FirstOrDefault();
                string username = acc.Name;

                db.Remove(acc);
                db.SaveChanges();
                return username;
            }
            catch
            {
                return null;
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MutiAdd()
        {
            return View();
        }
        public IActionResult DeleteAll()
        {

            List<Accounts> lines = new List<Accounts>();
            using var db = new AppDbContext();
            var accounts = db.Accounts.ToList();
            db.RemoveRange(accounts);
            db.SaveChanges();
            return RedirectToAction("Index", "Library", new { msg = "delete" });

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult MutiAdd(IFormFile fileToUpload)
        {
            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                List<Accounts> lines = new List<Accounts>();
                using var db = new AppDbContext();
                using (var stream = fileToUpload.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Insert(line, "");
                        }
                    }
                }

                //db.AddRange(lines);
                //db.SaveChanges();

                // Return a success page
                return RedirectToAction("Index", "Account", new { msg = "added" });
            }
            else
            {
                // User did not select a file
                return View("Index");
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string book, string author)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Library");
            }

            switch (Insert(book, author))
            {
                case true:
                    return RedirectToAction("Index", "Account", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "Account", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }

        public bool? Insert(string Account, string author)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.Accounts.Any(x => x.Name == Account))
                {
                    int? yearVal = null;

                    db.Add(new Accounts() { Name = Account });

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


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                var AccountTotal = db.Accounts.Count(x => x.Name == book);
                var isRenamed = AccountTotal == 0;
                var isOldName = AccountTotal == 1;


                //AccountTotal == 0 is a new name
                if ((isOldName || isRenamed))
                {

                    UpdateAccount(id, book, db);
                    return RedirectToAction("Update", "Account", new { id, msg = "updated" });
                }
                else if (AccountTotal > 1)
                {
                    return RedirectToAction("Update", "Account", new { id, error = "wrongName" });
                }
                else
                {
                    return RedirectToAction("Index", "Library", new { id, error = "wrongAccount" });
                }
            }
            catch
            {
                return RedirectToAction("Update", "Home", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen Account
        private static void UpdateAccount(int id, string Account, AppDbContext db)
        {
            var AccountData = db.Accounts.FirstOrDefault(x => x.BookId == id);
            AccountData.Name = Account;


            db.Update(AccountData);
            db.SaveChanges();
        }

        public IActionResult Update(int id)
        {
            using var db = new AppDbContext();
            var Account = db.Accounts.FirstOrDefault(x => x.BookId == id);
            if (Account != null)
            {

                return View(new AccountUpdateModel() {
                    ID = id,
                    Book = Account.Name,
                });
            }
            else
            {
                return RedirectToAction("Index", "Library", new { error = "wrongAccount" });
            }
        }
        public IActionResult DeleteAccount(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                db.Remove(new Accounts() { BookId = id });
                db.SaveChanges();
                return RedirectToAction("Index", "Library", new { msg = "AccountDeleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }
        public static List<string> ReadTextToList(string filename)
        {
            // Use a try-catch block for error handling
            try
            {
                // Open the file in read mode
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(filename))
                {
                    // Read all lines and add them to the list
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                return lines;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during reading
                Console.WriteLine($"Error reading file: {ex.Message}");
                return new List<string>();
            }
        }
    }
}
