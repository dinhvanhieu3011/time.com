using System;
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
    public class LibraryController : Controller
    {
        const int RETURN_DAYS = 2;
        //const string SUBJECT_CANCEL = "Reservation canceled";
        //const string BODY_CANCEL = "Your reservation of {0} has been canceled.";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISchedulerService _schedulerService;

        public LibraryController(IHttpContextAccessor httpContextAccessor, ISchedulerService schedulerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _schedulerService = schedulerService;
        }

        public IActionResult LogOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {

            //await _schedulerService.AutoTrecking();
            using var db = new AppDbContext();

            var booksAvailable = new AvailableBooksViewModel();
            //booksAvailable.nextExcuteJob = _schedulerService.GetNextExecutionTime().Value.ToLocalTime().AddMinutes(1);
            //booksAvailable.remainingSeconds = (DateTime.UtcNow - _schedulerService.GetNextExecutionTime().Value).TotalMilliseconds;
            foreach (var book in db.Books)
            {
                var total = db.BooksCopies.Count(x => x.BookId == book.BookId);

                var totalCurrentBook = db.ReservedBook.Count(x => x.BookId == book.BookId && x.ReturnedDate == null);

                booksAvailable.AvailableBooks.Add(new AvailableBooksModel()
                {
                    Book = book.Name,
                    BookId = book.BookId,
                    Author = book.Author,
                    TotalAccount = db.Channels.Where(X=>X.BookId == book.BookId).Count(),
                    AccountStatusOK = db.Channels.Where(X => X.BookId == book.BookId && X.Status == 1).Count() ,
                    AccountStatusDIE = db.Channels.Where(X => X.BookId == book.BookId && X.Status == 0).Count(),
                    ActionOVERVIDEO = db.Channels.Where(X => X.BookId == book.BookId && X.Action == 2).Count(),
                    ActionKET = db.Channels.Where(X => X.BookId == book.BookId && X.Action == 3).Count(),
                    ActionCHECKACC = db.Channels.Where(X => X.BookId == book.BookId && X.Action == 4).Count(),
                });
            }

            foreach (var reservations in db.ReservedBook)
            {
                var user = db.Users.FirstOrDefault(x => x.UserId == reservations.UserId).Username;

                var book = db.Books.FirstOrDefault(x => x.BookId == reservations.BookId);

                string barcode = "";

                if (reservations.BooksCopiesId != null)
                {
                    barcode = db.BooksCopies.FirstOrDefault(x => x.BooksCopiesId == reservations.BooksCopiesId).Barcode;
                }

                booksAvailable.ReservedBooks.Add(new ReservedBooksModel()
                {
                    Book = book.Name,
                    Author = book.Author,
                    ReservationId = reservations.ReservedBookId,
                    User = user,
                    Date = reservations.ReservedDate.ToString("yyyy-MM-dd hh:mm"),
                    CollectedDate = reservations.CollectedDate?.ToString("yyyy-MM-dd hh:mm"),
                    ReturnDate = reservations.ReturnDate?.ToString("yyyy-MM-dd hh:mm"),
                    ReturnedDate = reservations.ReturnedDate?.ToString("yyyy-MM-dd hh:mm"),
                    Barcode = barcode
                });
            }

            foreach (var users in db.Users)
            {
                booksAvailable.Users.Add(new UsersModel
                {
                    Username = users.Username,
                    Role = users.Role == 0 ? "Admin" : "Customer",
                    UserId = users.UserId,
                    Registered = users.Registered.ToString("yyyy-MM-dd"),
                    Email = users.Email
                });
            }
            foreach (var users in db.Accounts)
            {
                booksAvailable.Accounts.Add(new Accounts
                {
                    Name = users.Name,
                    BookId = users.BookId,
                });
            }
            foreach (var channel in db.Channels)
            {
                booksAvailable.Channels.Add(new ChannelModel
                {
                    ChannelId = channel.ChannelId,
                    BookId = channel.BookId,
                    BookName = db.Books.Where(x=>x.BookId == channel.BookId).FirstOrDefault()?.Name,
                    Name = channel.Name,
                    Status = channel.Status,
                    StatusName =channel.Status == 1 ? "OK" : "DIE",
                    Action = channel.Action, 
                    ActionName = MapAction (channel.Action ),
                    VideoCount = channel.VideoCount ,
                    LastVideoCreatedDate = channel.LastVideoCreatedDate ,
                    LastVideoCreatedDateString = channel.LastVideoCreatedDate.Year >2000 ? Helper.TimeAgo(channel.LastVideoCreatedDate.ToLocalTime()):"",
                    ViewCount = channel.ViewCount,
                    LikeCount = channel.LikeCount,
                    CreatedDate = channel.CreatedDate,
                    CreatedDateString = Helper.TimeAgo(channel.CreatedDate)
                });;
            }

            return View(booksAvailable);
        }
        private string ReturnRemainString(DateTime fromDate, DateTime toDate)
        {


            // Cập nhật thời gian còn lại

            double remainingSeconds = (toDate - fromDate).TotalMinutes ;

            // Hiển thị thời gian còn lại
            double hours = Math.Round(remainingSeconds / 60, 0, MidpointRounding.ToZero) ;
            double minutes = Math.Round(remainingSeconds  - hours * 60);
            if(hours == 0)
            {           
                return string.Format("{0} phút trước", minutes);
            }else
            {
                return string.Format("{0} giờ,{1} phút trước", hours, minutes);
            }
        }
        private string MapAction (int Action)
        {
            switch (Action)
            {
                case 0: return "THAY ACC"; 
                case 2: return "DƯ VIDEO"; 
                case 3: return "ACC KẸT"; 
                case 4: return "CHECK ACC"; 
                default: return "OK"; 
            }
        }
        public IActionResult DeleteUser(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                var user = _httpContextAccessor.HttpContext.Session.GetString("user");
                using var db = new AppDbContext();

                if (db.Users.Count(x => x.Role == 0) == 1)
                {
                    return RedirectToAction("Index", "Library", new { error = "oneAdmin" });
                }
                else if (id == db.Users.FirstOrDefault(x => x.Username == user).UserId)
                {
                    return RedirectToAction("Index", "Library", new { error = "sameUser" });
                }
                else if (!db.ReservedBook.Any(x => x.UserId == id))
                {
                    db.Remove(new Users() { UserId = id });
                    db.SaveChanges();

                    return RedirectToAction("Index", "Library", new { msg = "userDeleted" });
                }
                return RedirectToAction("Index", "Library", new { error = "reserved" });
            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }

        public IActionResult DeleteBook(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();
                db.ReservedBook.RemoveRange(db.ReservedBook.ToList());
                db.BooksCopies.RemoveRange(db.BooksCopies.ToList());
                db.SaveChanges();

                db.Remove(new Books() { BookId = id });
                db.SaveChanges();
                return RedirectToAction("Index", "Library", new { msg = "bookDeleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }
        public IActionResult DeleteChannel(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                db.Remove(new Channels() { ChannelId = id });
                db.SaveChanges();
                return RedirectToAction("Index", "Library", new { msg = "ChannelDeleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }
        public IActionResult CollectBook(int id, string barcode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                var currentBC = db.BooksCopies.FirstOrDefault(x => x.BookId == id && x.Barcode == barcode && x.CanBeReserved == 1);

                if (currentBC == null)
                {
                    return RedirectToAction("Index", "Library", new { error = "wrongBarcode" });
                }

                if (db.ReservedBook.Any(x => x.BooksCopiesId == currentBC.BooksCopiesId && x.ReturnedDate == null))
                {
                    return RedirectToAction("Index", "Library", new { error = "reservedBook" });
                }

                var reservedBook = db.ReservedBook.FirstOrDefault(x => x.ReservedBookId == id);

                reservedBook.BooksCopiesId = currentBC.BooksCopiesId;

                reservedBook.CollectedDate = DateTime.Now;

                reservedBook.ReturnDate = DateTime.Now.AddDays(RETURN_DAYS);

                db.Update(reservedBook);

                db.SaveChanges();

                return RedirectToAction("Index", "Library", new { msg = "collected" });
            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }

        public IActionResult ReturnedBook(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                var reservedBook = db.ReservedBook.FirstOrDefault(x => x.ReservedBookId == id);

                reservedBook.ReturnedDate = DateTime.Now;

                db.Update(reservedBook);

                db.SaveChanges();

                return RedirectToAction("Index", "Library", new { msg = "returned" });
            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }

        public IActionResult CancelReservation(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }

                using var db = new AppDbContext();

                //Uncomment it to send emails
                //var reservedBook = db.ReservedBook.FirstOrDefault(x => x.ReservedBookId == id);
                //var email = db.Users.FirstOrDefault(x => x.UserId == reservedBook.UserId).Email;
                //var book = db.Books.FirstOrDefault(x => x.BookId == reservedBook.BookId).Name;
                //MailLibrary.MailNotifications.SendEmail(email, SUBJECT_CANCEL, string.Format(BODY_CANCEL, book));

                db.Remove(new ReservedBook() { ReservedBookId = id });
                db.SaveChanges();

                return RedirectToAction("Index", "Library", new { msg = "reservationCanceled" });
            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }
    }
}
