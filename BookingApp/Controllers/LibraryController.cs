using System;
using System.Linq;
using BASE.Data.Interfaces;
using BookingApp.Filters.Authorization;
using BookingApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    public class LibraryController : Controller
    {
        const int RETURN_DAYS = 2;
        //const string SUBJECT_CANCEL = "Reservation canceled";
        //const string BODY_CANCEL = "Your reservation of {0} has been canceled.";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISchedulerService _schedulerService;
		private readonly IUsersDTRepository _usersDTRepository;

		public LibraryController(IHttpContextAccessor httpContextAccessor, ISchedulerService schedulerService, IUsersDTRepository usersDTRepository )
        {
            _httpContextAccessor = httpContextAccessor;
            _schedulerService = schedulerService;
            _usersDTRepository = usersDTRepository;

		}

        public IActionResult LogOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
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

                if (_usersDTRepository.GetAll().Count(x => x.Role == 0) == 1)
                {
                    return RedirectToAction("Index", "Library", new { error = "oneAdmin" });
                }
                else if (id == _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == user).UserId)
                {
                    return RedirectToAction("Index", "Library", new { error = "sameUser" });
                }
                return RedirectToAction("Index", "Library", new { error = "reserved" });
            }
            catch
            {
                return RedirectToAction("Index", "Library", new { error = "error" });
            }
        }
    }
}
