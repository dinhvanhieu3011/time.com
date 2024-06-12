using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    [Authorize(Roles.CUSTOMER, Roles.ADMIN)]
    public class ProfileController : Controller
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProfileController(IHttpContextAccessor httpContextAccessor, IUsersDTRepository usersDTRepository, IUnitOfWork unitOfWork )
        {
            _httpContextAccessor = httpContextAccessor;
            _usersDTRepository = usersDTRepository;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string email = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == _httpContextAccessor.HttpContext.Session.GetString("user")).Email;
            return View(new ProfileModel
            {
                Email = email
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string email)
        {
            try
            {


                var isEmailInvalid = _usersDTRepository.GetAll().Count(x => x.Email == email) > 0;

                var user = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == _httpContextAccessor.HttpContext.Session.GetString("user"));

                if (isEmailInvalid && !user.Email.Equals(email))
                {
                    return RedirectToAction("Index", "Profile", new { error = "registered" });
                }

                user.Email = email;
                _usersDTRepository.Update(user);
                _unitOfWork.Complete();

                return RedirectToAction("Index", "Profile", new { msg = "updated" });
            }
            catch
            {
                return RedirectToAction("Index", "Profile", new { error = "error" });
            }
        }
    }
}