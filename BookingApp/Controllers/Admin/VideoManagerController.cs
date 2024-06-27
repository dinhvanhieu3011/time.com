using BASE.CORE.Helper;
using BASE.Data.Implements;
using BASE.Data.Implements.DexTrack;
using BASE.Data.Interfaces;
using BASE.Data.Interfaces.DexTrack;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using BASE.Model;
using BASE.Model.Dextrack;
using BookingApp.Models;
using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;
using static BookingApp.Controllers.WhatsappController;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookingApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VideoManagerController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        private readonly IComputerRepository _computerRepository;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUserActionRepository _userActionRepository;
        private readonly IVideosRepository _videosRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWhatsAppChatRepository _whatsAppChatRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VideoManagerController(IComputerRepository computerRepository, IUsersDTRepository usersDTRepository,
            IUserSessionRepository userSessionRepository, IUserActionRepository userActionRepository,
            IVideosRepository videosRepository,
            IHostEnvironment env, ILogger<UploadController> logger,
             IUnitOfWork unitOfWork, IWhatsAppChatRepository whatsAppChatRepository, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _logger = logger;
            _computerRepository = computerRepository;
            _usersDTRepository = usersDTRepository;
            _userSessionRepository = userSessionRepository;
            _userActionRepository = userActionRepository;
            _videosRepository = videosRepository;
            _unitOfWork = unitOfWork;
            _whatsAppChatRepository = whatsAppChatRepository;
            _httpContextAccessor = httpContextAccessor;


        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private readonly JsonResponse response = new JsonResponse();
        [HttpGet]
        public JsonResponse getList(int? id, string Ngay)
        {
            CultureInfo provider = CultureInfo.CurrentCulture;
            DateTime dateTime = new DateTime();
            if (!string.IsNullOrEmpty(Ngay))
                dateTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider);
            else
                dateTime = DateTime.Now;
            var model = new VideoModel();
            if (id.HasValue)
            {
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
                                   Thumbnail = g.FirstOrDefault().VideoPath,
                               })
                               .OrderByDescending(x=>x.Hours).ToList();
                this.response.Data = lstVideo;
                this.response.Success = true;
                return this.response;
            }
            this.response.Success = false;
            return this.response;
        }
    }
}
