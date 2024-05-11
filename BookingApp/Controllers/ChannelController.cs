using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.DB.Classes.DB;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class ChannelController : Controller
    {
        public ChannelController()
        {

        }

        public IActionResult Index()
        {
            using var db = new AppDbContext();
            return View(new Models.ChannelCreateModel()
            {
            ChannelsModel = db.Books.ToList().Select
                (
                x => new SelectListItem { Value = x.BookId.ToString(), Text = x.Name }
                ).ToList(),
            SelectedChannel = 1});
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string name, string SelectedChannel)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Library");
            }

            switch (Insert(name, int.Parse(SelectedChannel)))
            {
                case true:
                    return RedirectToAction("Index", "Channel", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "Channel", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }

        public bool? Insert(string name, int  bookId)
        {
            try
            {
                using var db = new AppDbContext();


                    db.Add(new Channels() 
                    { 
                        Name = name,
                        BookId = bookId,
                        CreatedDate = DateTime.Now
                    });

                    db.SaveChanges();


                    return true;
            }
            catch
            {
                return null;
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string book, string author, string year, IList<string> barcodes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                var bookTotal = db.Books.Count(x => x.Name == book);
                var isRenamed = bookTotal == 0;
                var isOldName = bookTotal == 1;

                var totalReserved = db.ReservedBook.Count(x => x.BookId == id);

                var copiesMakeLogic = (db.BooksCopies.Count(x => x.BookId == id) - totalReserved) >= 0;

                //bookTotal == 0 is a new name
                if ((isOldName || isRenamed) && copiesMakeLogic)
                {
                    int? yearVal = null;

                    if (!string.IsNullOrEmpty(year))
                    {
                        yearVal = int.Parse(year);
                    }

                    UpdateBook(id, book, author, yearVal, barcodes, db);
                    return RedirectToAction("Update", "Book", new { id, msg = "updated" });
                }
                else if (!copiesMakeLogic)
                {
                    return RedirectToAction("Update", "Book", new { id, error = "lessBooks" });
                }
                else if (bookTotal > 1)
                {
                    return RedirectToAction("Update", "Book", new { id, error = "wrongName" });
                }
                else
                {
                    return RedirectToAction("Index", "Library", new { id, error = "wrongBook" });
                }
            }
            catch
            {
                return RedirectToAction("Update", "Home", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private static void UpdateBook(int id, string book, string author, int? year, IList<string> barcodes, AppDbContext db)
        {
            var bookData = db.Books.FirstOrDefault(x => x.BookId == id);
            bookData.Name = book;
            bookData.Author = author;
            bookData.PublicationYear = year;

            db.Update(bookData);
            db.SaveChanges();

            var currentBarcodes = db.BooksCopies.Where(x => x.BookId == id).Select(x => x.Barcode).ToList();

            var newBarcodes = barcodes.Except(currentBarcodes).ToList();


            var removedBarcodes = currentBarcodes.Except(barcodes).ToList();

            foreach (var barcode in removedBarcodes)
            {
                var booksCopiesId = db.BooksCopies.FirstOrDefault(x => x.Barcode == barcode).BooksCopiesId;
                if (!db.ReservedBook.Any(x => x.BooksCopiesId == booksCopiesId))
                {
                    using var db2 = new AppDbContext();
                    db2.Remove(new BooksCopies { BooksCopiesId = booksCopiesId });
                    db2.SaveChanges();
                }
            }
        }

        public IActionResult Update(int id)
        {
            using var db = new AppDbContext();
            var book = db.Books.FirstOrDefault(x => x.BookId == id);
            if (book != null)
            {
                var barcodes = (from bookCopies in db.BooksCopies where bookCopies.BookId == id orderby bookCopies.BooksCopiesId select new { id = bookCopies.BooksCopiesId, barcode = bookCopies.Barcode }).ToDictionary(item => item.id, item => item.barcode);

                return View(new Models.BookUpdateModel() {
                    ID = id,
                    Book = book.Name,
                    Author = book.Author,
                    Year = book.PublicationYear,
                    Barcodes = barcodes,
                    Total = barcodes.Count
                });
            }
            else
            {
                return RedirectToAction("Index", "Library", new { error = "wrongBook" });
            }
        }
    }
}
