using System;
using System.Collections.Generic;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Model;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
namespace BookingApp.Controllers
{
    public class VideoController : Controller
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVideosRepository _videosRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUserActionRepository _userActionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IComputerRepository _computerRepository;

        private readonly JsonResponse response = new JsonResponse();
        readonly IHostEnvironment _env;
        public VideoController(IHostEnvironment env, IHttpContextAccessor httpContextAccessor, IVideosRepository videosRepository, IUserSessionRepository userSessionRepository
            , IUserActionRepository userActionRepository, IComputerRepository computerRepository )
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _videosRepository = videosRepository;
            _userSessionRepository = userSessionRepository;
            _userActionRepository = userActionRepository;
            _computerRepository = computerRepository;
        }
        public IActionResult List(int id)
        {
            ViewBag.id = id;
            return View();

        }
        public IActionResult Dashboard()
        {
            var coms = _videosRepository.GetAll().ToList();
            return View(coms);

        }
        public IActionResult Index(int id)
        {
            var data = _videosRepository.GetAll().Where(x=>x.ChannelId == id&& x.IsDelete == 0)
                .OrderByDescending(x=>x.Id).ToList();

            return View(data);

        }
        public VideoModel Get(string id)
        {
            var model = new VideoModel();
            var book = _videosRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
            var list = new List<string>();
            list.Add( book.VideoPath.Replace(@"\", @"/"));
            string rootPath = _env.ContentRootPath;
            var videoPath = Helper.CreateMasterM3U8(rootPath,list).Replace(@"\", @"/");
            var userSession = _userSessionRepository.GetAll().Where(x => x.VideoId == id).ToList();
            var userAction = _userActionRepository.GetAll().Where(x => x.VideoId == id).ToList();
            model.userSessions = userSession;
            model.Video = book;
            model.userActions = userAction;
            return model;
        }
        public IActionResult Detail(string id)
        {
            var model = new VideoModel();  
            if(string.IsNullOrEmpty(id))
            {
                var book = _videosRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                var channel = _computerRepository.GetAll().Where(x => x.Id == book.ChannelId).FirstOrDefault();
                var relatedVideo = _videosRepository.GetAll().Where(x => x.ChannelId == channel.Id && x.Year == book.Year
                && x.Month == book.Month && x.Date == book.Date && x.IsDelete == 0 && x.Id != id)
                    .OrderByDescending(x => x.Id).Take(5).ToList();
                var userSession = _userSessionRepository.GetAll().Where(x => x.VideoId == id).ToList();
                var userAction = _userActionRepository.GetAll().Where(x => x.VideoId == id).ToList();
                model.userSessions = userSession;
                model.Video = book;
                model.userActions = userAction;
                if (book.IsMerge == 1)
                {
                    ViewBag.filePath = book.VideoPath.Replace(@"\", @"/");

                }
                else
                {
                    var list = new List<string>();
                    list.Add(book.VideoPath.Replace(@"\", @"/"));
                    string rootPath = _env.ContentRootPath;
                    var videoPath = Helper.CreateMasterM3U8(rootPath, list);
                    ViewBag.filePath = videoPath.Replace(@"\", @"/");
                }
                ViewBag.relatedVideo = relatedVideo;
                ViewBag.channel = channel.Name;
            }    

            return View(model);
        }

        private void createM3u8(string v)
        {

            throw new NotImplementedException();
        }
    }
}
