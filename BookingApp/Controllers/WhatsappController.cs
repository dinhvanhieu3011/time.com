﻿using BASE.Data.Implements;
using BASE.Data.Interfaces;
using BASE.Data.Interfaces.DexTrack;
using BASE.Data.Migrations;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BookingApp.Controllers
{

    public class WhatsappController : Controller
    {
		private readonly ILogger<WhatsappController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IWhatsAppChatRepository _repository;
		private readonly IUsersDTRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WhatsappController(ILogger<WhatsappController> logger, IConfiguration configuration, 
			IWhatsAppChatRepository repository, IUnitOfWork unitOfWork, IUsersDTRepository userRepository, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_configuration = configuration;
			_repository = repository;
			_unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Index(string myNum)
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
                            PhoneNumer = g.FirstOrDefault().ToPhoneNumber != myNum ? g.FirstOrDefault().ToPhoneNumber : g.FirstOrDefault().FromPhoneNumber,
                            LastMessage = g.OrderByDescending(p => p.Time).FirstOrDefault().Message,
                            whatsAppChats = g.OrderBy(x => x.Time).ToList()
                        }).ToList();
                if (listChat.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.MyNumber = myNum;


                return View(listChat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
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
    }
}
