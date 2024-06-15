﻿using System.Diagnostics;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.IdentityAccess;
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
            if (role == 0)
            {
                return RedirectToAction("Index", "User");
            }
            else if (role == 1)
            {
                RedirectToAction("Index", "Computer");
            }
            else
            {
                RedirectToAction("Index", "Whatsapp");
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
                if(user.Role == 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else if(user.Role == 1)
                {
                    return RedirectToAction("Index", "Computer");
                }
                else
                {
                    return RedirectToAction("Index", "Whatsapp");
                }
            }

            return RedirectToAction("Index", "Home", new { error = "password" });
        }
    }
}
