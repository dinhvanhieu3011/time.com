using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using BASE.Model;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
        public IActionResult List_v2(int? id)
        {
            CultureInfo provider = CultureInfo.CurrentCulture;
            DateTime dateTime = DateTime.ParseExact("14/06/2024", "dd/MM/yyyy", provider);
            var lstVideo = _videosRepository.GetAll()
               .Where(x => x.IsDelete == 0 && x.ChannelId == id && x.Start.Date == dateTime.Date)
               .OrderBy(x => x.Start)
               .GroupBy(p => new { p.Year, p.Month, p.Date, p.Hours })
               .Select(g => new Videos
               {
                   Year = g.Key.Year,
                   Month = g.Key.Month,
                   Date = g.Key.Date,
                   Hours = g.Key.Hours,
                   Thumbnail = g.FirstOrDefault().VideoPath
               }).OrderByDescending(x=>x.Hours)
               .ToList();
            ViewBag.id = id;
            ViewBag.relatedVideo = lstVideo;
            return View();

        }
        public IActionResult Dashboard()
        {
            var coms = _computerRepository.GetAll().ToList();
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
            var list = new List<Videos>();
            list.Add( book);
            string rootPath = _env.ContentRootPath;
            var videoPath = Helper.CreateMasterM3U8(rootPath, list).Replace(@"\", @"/");
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
            if(!string.IsNullOrEmpty(id))
            {
                Videos book = new Videos();
                if (id.Contains("unmerge"))
                {
                    var arr = id.Split("-");
                    //Id = "unmerge-" + g.Key.Year + "-" + g.Key.Month + "-" + g.Key.Date + "-" + g.Key.Hours + "-" + id
                    var lstbook = _videosRepository.GetAll().Where(x => x.IsDelete == 0 && x.IsMerge == 0 &&
                    x.ChannelId == int.Parse(arr[5]) && x.Year == int.Parse(arr[1])
                && x.Month == int.Parse(arr[2]) && x.Date == int.Parse(arr[3]) && x.Hours == int.Parse(arr[4]) && x.IsDelete == 0 && x.Id != id);
                    book = lstbook.FirstOrDefault();
                    var lstId = lstbook.Select(x=>x.Id).ToList();
                    //book = _videosRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                    var channel = _computerRepository.GetAll().Where(x => x.Id == book.ChannelId).FirstOrDefault();
                    var relatedVideo = _videosRepository.GetAll().Where(x => x.ChannelId == channel.Id && x.Year == book.Year
                    && x.Month == book.Month && x.Date == book.Date && x.IsDelete == 0 && x.Id != id)
                        .OrderByDescending(x => x.Id).Take(5).ToList();
                    var userSession = _userSessionRepository.GetAll().Where(x => lstId.Contains(x.VideoId)).ToList();
                    var userAction = _userActionRepository.GetAll().Where(x => lstId.Contains(x.VideoId)).ToList();
                    model.userSessions = userSession;
                    model.Video = book;
                    model.userActions = userAction;

                    var list = lstbook.Select(x => x.VideoPath.Replace(@"\", @"/")).ToList();
                        string rootPath = _env.ContentRootPath;
                        var videoPath = Helper.CreateMasterM3U8(rootPath, lstbook.ToList());
                        ViewBag.filePath = videoPath.Replace(@"\", @"/");
                  
                    ViewBag.relatedVideo = relatedVideo;
                    ViewBag.channel = channel.Name;
                }
                else
                {
                     book = _videosRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                    //book = _videosRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
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


            }    

            return View(model);
        }

        private void createM3u8(string v)
        {

            throw new NotImplementedException();
        }
    }
}
