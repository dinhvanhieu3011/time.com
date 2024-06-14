using System;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using BookingApp.Filters.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    [Authorize(Roles.ADMIN)]
    public class UserController : Controller
    {
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IHttpContextAccessor httpContextAccessor, IUsersDTRepository usersDTRepository, IUnitOfWork unitOfWork)
        {
            _usersDTRepository = usersDTRepository;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var list = _usersDTRepository.GetAll().ToList();
            return View(list);
        }
        #region create
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string username, string password, string phone, string teleToken, string chatId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "User");
            }

            switch (Insert(username, password, phone, teleToken, chatId))
            {
                case true:
                    return RedirectToAction("Index", "User", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "User", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "User", new { error = "error" });
            }
        }

        public bool? Insert(string username, string password, string phone, string teleToken, string chatId)
        {
            try
            {

                if (!_usersDTRepository.GetAll().Any(x => x.Username == username) 
                    && !string.IsNullOrEmpty(username) 
                    && !string.IsNullOrEmpty(password)
                    && !string.IsNullOrEmpty(teleToken)
                    && !string.IsNullOrEmpty(chatId)
                    )
                {
                    _usersDTRepository.Insert(new Users()
                    {
                        Username = username,
                        Password = password,
                        Email = phone,
                        Role = 1,
                        Registered = DateTime.Now,
                        TeleToken = teleToken,
                        ChatId = chatId,
                    });

                    _unitOfWork.Complete();


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
            var book = _usersDTRepository.GetAll().FirstOrDefault(x => x.UserId == id);
            if (book != null)
            {

                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "User", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string username, string password, string phone, string teleToken, string chatId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }


                if (id > 0 && !string.IsNullOrEmpty(username) 
                    && !string.IsNullOrEmpty(password)
                    && !string.IsNullOrEmpty(password)
                    && !string.IsNullOrEmpty(teleToken)
                    && !string.IsNullOrEmpty(chatId)
                    )
                {

                    UpdateData(id, username, password, phone, teleToken, chatId);
                    return RedirectToAction("Update", "User", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "User", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private void UpdateData(int id, string username, string password, string phone, string teleToken, string chatId)
        {
            var data = _usersDTRepository.GetAll().FirstOrDefault(x => x.UserId == id);
            data.Username = username;
            data.Password = password;
            data.TeleToken = teleToken;
            data.ChatId = chatId;
            data.Email = phone;

            _usersDTRepository.Update(data);
            _unitOfWork.Complete(); 
        }
        #endregion
        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "User");
                }

                _usersDTRepository.Delete(new Users() { UserId = id });
                _unitOfWork.Complete();
                return RedirectToAction("Index", "User", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "User", new { error = "error" });
            }
        }
    }
}
