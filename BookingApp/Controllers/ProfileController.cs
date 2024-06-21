using System.Collections.Generic;
using System;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BASE.Data.Interfaces.DexTrack;

namespace BookingApp.Controllers
{
    [Authorize(Roles.CUSTOMER, Roles.ADMIN)]
    public class ProfileController : Controller
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUnitOfWork _unitOfWork;
		private readonly IWhatsAppChatRepository _repository;

		public ProfileController(IHttpContextAccessor httpContextAccessor, IUsersDTRepository usersDTRepository, IUnitOfWork unitOfWork , IWhatsAppChatRepository whatsAppChatRepository )
        {
            _httpContextAccessor = httpContextAccessor;
            _usersDTRepository = usersDTRepository;
            _unitOfWork = unitOfWork;
			_repository = whatsAppChatRepository;
        }

        public IActionResult Index()
        {
            string email = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == _httpContextAccessor.HttpContext.Session.GetString("user")).Email;
            return View(new ProfileModel
            {
                Email = email
            });
        }
        public IActionResult List()
        {
            return View();
        }
		public IActionResult Detail(string id)
		{
			try
			{
				var listMessage = _repository.GetAll().ToList();
				//var username = _httpContextAccessor.HttpContext.Session.GetString("user");

				//var user = _userRepository.GetAll().FirstOrDefault(x => x.Username == username);
				//var myNum = user.Email;

				var listChat = listMessage.OrderByDescending(x => x.Time).GroupBy(p => p.ChatId)
						.Select(g => new WhatsAppChatModel
						{
							Key = g.Key,
							PhoneNumer = g.FirstOrDefault().ToPhoneNumber != id ? g.FirstOrDefault().ToPhoneNumber : g.FirstOrDefault().FromPhoneNumber,
							LastMessage = g.OrderByDescending(p => p.Time).FirstOrDefault().Message,
							whatsAppChats = g.OrderBy(x => x.Time).ToList()
						}).ToList();
				if (listChat.Count == 0)
				{
					return RedirectToAction("Index", "Home");
				}
				ViewBag.MyNumber = id;


				return View(listChat);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Home");
			}

		}
		public class WhatsAppChatModel
		{
			public string Key { get; set; }
			public string LastMessage { get; set; }
			public string PhoneNumer { get; set; }
			public List<WhatsAppChat> whatsAppChats { get; set; }
		}
		public class ChatModel
		{
			public string Sender { get; set; }
			public string Time { get; set; }
			public string UserName { get; set; }

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