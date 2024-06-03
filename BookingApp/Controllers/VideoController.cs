﻿using System;
using System.Linq;
using BookingApp.DB.Classes.DB;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BookingApp.Controllers
{
    public class VideoController : Controller
    {
        public class JsonResponse
        {
            public object Data { get; set; }

            public string Message { get; set; }

            public bool Success { get; set; }

            public string Pager { get; set; }

            public string Id { get; set; }
        }
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonResponse response = new JsonResponse();

        public VideoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult List(int id)
        {
            ViewBag.id = id;
            return View();

        }
        public IActionResult Index(int id)
        {
            using var db = new AppDbContext();
            var data = db.Videos.Where(x=>x.ChannelId == id&& x.IsDelete == 0)
                .OrderByDescending(x=>x.Id).ToList();

            return View(data);

        }
        public VideoModel Get(int id)
        {
            var db = new AppDbContext();
            var model = new VideoModel();
            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
            book.VideoPath = @"/" + book.VideoPath.Replace(@"\", @"/");
            var userSession = db.UserSessions.Where(x => x.VideoId == id).ToList();
            var userAction = db.UserActions.Where(x => x.VideoId == id).ToList();
            model.userSessions = userSession;
            model.Video = book;
            model.userActions = userAction;
            return model;
        }
        public IActionResult Detail(int id)
        {
            var db = new AppDbContext();
            var model = new VideoModel();   
            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
            var channel = db.ChannelYoutubes.Where(x => x.Id == book.ChannelId).FirstOrDefault();
            var relatedVideo = db.Videos.Where(x => x.ChannelId == channel.Id && x.Year == book.Year 
            && x.Month == book.Month && x.Date == book.Date && x.IsDelete == 0 && x.Id != id)
                .OrderByDescending(x => x.Id).Take(5).ToList();
            var userSession = db.UserSessions.Where(x => x.VideoId == id).ToList();
            var userAction = db.UserActions.Where(x => x.VideoId == id).ToList();
            model.userSessions = userSession;
            model.Video = book;
            model.userActions = userAction;

            ViewBag.relatedVideo = relatedVideo;
            ViewBag.channel = channel.Name;
            ViewBag.filePath = @"/" + book.VideoPath.Replace(@"\",@"/");
            return View(model);
        }

    }
}
