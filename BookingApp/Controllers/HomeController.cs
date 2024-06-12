using System.Diagnostics;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BookingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

namespace BookingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IHttpContextAccessor httpContextAccessor,
            IUsersDTRepository usersDTRepository,
            IUnitOfWork unitOfWork
            /*, ILogger<HomeController> logger*/)
        {
            //_logger = logger;

            _httpContextAccessor = httpContextAccessor;
            _usersDTRepository = usersDTRepository;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {


            var role = _httpContextAccessor.HttpContext.Session.GetInt32("role");
            if (role != null)
            {
                return RedirectToAction("Index", "Computer");
            }

            return View();
        }

        public IActionResult FirstTime()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error500()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = _usersDTRepository.GetAll().Where(x => x.Username == username && x.Password == password).FirstOrDefault();

            if (user != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("JWToken", TokenProvider.LoginUser(user));

                _httpContextAccessor.HttpContext.Session.SetString("user", username);
                _httpContextAccessor.HttpContext.Session.SetInt32("role", user.Role);

                return user.Role == 0 ? RedirectToAction("Index", "Computer") : (IActionResult)RedirectToAction("Index", "File");
            }

            return RedirectToAction("Index", "Home", new { error = "password" });
        }
    }
}
