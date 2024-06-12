using BMBSOFT.GIS.CORE.Helper;
using BookingApp.DB.Classes.DB;
using BookingApp.Models;
using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static BookingApp.Controllers.VideoController;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Collections.Specialized.BitVector32;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        public class JsonResponse
        {
            public object Data { get; set; }

            public string Message { get; set; }

            public bool Success { get; set; }

            public string Pager { get; set; }

            public string Id { get; set; }
        }
        private bool? Insert(string Name, string Link, string Category, int CookaAccountId, string UserId, int CategoryId)
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.ChannelYoutubes.Any(x => x.Link == Link)
                    && !string.IsNullOrEmpty(Name)
                    )
                {
                    db.Add(new ChannelYoutubes()
                    {
                        Name = Name,
                        Link = Link,
                        Category = Category,
                        CookaAccountId = CookaAccountId,
                        UserId = UserId,
                        CategoryId = CategoryId,
                    });

                    db.SaveChanges();


                    return true;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return false;
            }
        }
        private readonly JsonResponse response = new JsonResponse();
        public UploadController(IHostEnvironment env, ILogger<UploadController> logger)
        {
            _env = env;
            _logger = logger;
        }
        public class ImageDto
        {
            public IFormFile Image { set; get; }
            public string token { set; get; }

        }
        public class VideoDto
        {
            public IFormFile Video { set; get; }
            public IFormFile UserAction { set; get; }
            public IFormFile UserSession { set; get; }
            public string token { set; get; }
        }
        [HttpPost]
        public JsonResponse CreateComputer(string ComputerName, string Token, string EmployeeName)
        {
            if (!ModelState.IsValid)
            {
                this.response.Success = false; 
            }
            switch (Insert(ComputerName, Token, "", 0, EmployeeName, 0))
            {
                case true:
                    this.response.Success = true; break;
                case false:
                    this.response.Success = false; break;
                case null:
                    this.response.Success = false; this.response.Message = "Đã tồn tại token"; break;   
            }
            return this.response;
    
        }
        [HttpGet]
        public JsonResponse getDashboard(string Ngay, string channelId, int pageIndex, int pageSize)
        {
            try
            {
                var totalRow = 0;
                var channelIds = new string[100];
                if (!string.IsNullOrEmpty(channelId))
                {
                     channelIds = channelId.Split(',');

                }
                using var db = new AppDbContext();
                CultureInfo provider = CultureInfo.CurrentCulture;
                DateTime dateTime = DateTime.Now;
                if (!string.IsNullOrEmpty(Ngay))
                {
                    dateTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider);
                }

                var query = from session in db.UserSessions.OrderBy(s => s.StartTime)
                            join video in db.Videos on session.VideoId equals video.Id
                            join channel in db.ChannelYoutubes on video.ChannelId equals channel.Id
                            where video.IsDelete == 0 
                            && ( string.IsNullOrEmpty(Ngay) || dateTime.Date == video.Start.Date )
                            && ( string.IsNullOrEmpty(channelId) || channelIds.Contains(channel.Id.ToString()))
                            group session by new { channel.Name, session.Windows } into sessionGroup
                            select new UserSessionModel
                            {
                                ComputerName = sessionGroup.Key.Name,
                                Windows = sessionGroup.Key.Windows,
                                UseTime = sessionGroup.Sum(s => s.EndTime - s.StartTime).ToString(),
                                Start = convert(sessionGroup.Min(x=>x.StartTime)),
                                End = convert(sessionGroup.Max(x => x.EndTime))
                            };

                totalRow = query.ToList().Count();
                this.response.Data = query.ToList();
                this.response.Success = true;
                this.response.Pager = totalRow.ToString();
                return this.response;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.Message = ex.Message;
                return this.response;
            }
        }
        [HttpGet]
        public JsonResponse getListDataVideo(string KeyWord, int id, int pageIndex, int pageSize)
        {
            try
            {
                var totalRow = 0;
                if(!string.IsNullOrEmpty(KeyWord))
                    KeyWord = KeyWord.ToLower();
                using var db = new AppDbContext();
                var listUserActions = db.UserActions
                    .Where(x => x.VideoId == id)
                    .Where(x=> string.IsNullOrEmpty(KeyWord) || x.Keys.ToLower().Contains(KeyWord)
                    || x.Windows.ToLower().Contains(KeyWord)
                    || x.UserName.ToLower().Contains(KeyWord)
                    ).OrderBy(x => x.Time);
                totalRow = listUserActions.Count();
                this.response.Data = listUserActions.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                this.response.Success = true;
                this.response.Pager = totalRow.ToString();
                return this.response;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.Message = ex.Message;
                return this.response;
            }
        }
        [HttpGet]
        public JsonResponse getList(string Ngay, int id,  int pageIndex, int pageSize)
        {
            try
            {
                var totalRow = 0;
                List<Videos> data = new List<Videos>();
                CultureInfo provider = CultureInfo.CurrentCulture;
                if (string.IsNullOrEmpty(Ngay))
                {
                    using var db = new AppDbContext();
                    data = db.Videos.Where(x=>(x.ChannelId == id || id == 0) && x.IsDelete == 0).ToList();
                }
                else
                {
                    DateTime dateTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider);
                    using var db = new AppDbContext();
                    data = db.Videos.Where(x=> (x.ChannelId == id || id == 0) && x.Start.Date == dateTime && x.IsDelete == 0).ToList();
                }

                totalRow = data.Count();
                this.response.Data = data.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                this.response.Success = true;
                this.response.Pager = totalRow.ToString();
                return this.response;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.Message = ex.Message;
                return this.response;
            }
        }
        [HttpGet]
        public Videos Get(int id)
        {
            var db = new AppDbContext();

            var book = db.Videos.Where(x => x.Id == id).FirstOrDefault();
            if (book.IsMerge == 1)
            {
                book.VideoPath = book.VideoPath.Replace(@"\", @"/");

            }
            else
            {
                var list = new List<string>();
                list.Add(book.VideoPath.Replace(@"\", @"/"));
                string rootPath = _env.ContentRootPath;
                var videoPath = Helper.CreateMasterM3U8(rootPath, list);
                book.VideoPath = videoPath;
            }
            return book;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] ImageDto image)
        {
            try
            {

                string fPath = Path.Combine(_env.ContentRootPath, "live", image.token + ".png"); // Or use your preferred storage location
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    await image.Image.CopyToAsync(stream);

                }      

                return Ok("Videos uploaded successfully");
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideos([FromForm] VideoDto video)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var computers = db.ChannelYoutubes.ToList();
                    var com = computers.Where(x => x.Link == video.token).FirstOrDefault();
                    // Split the Video path using the backslash ('\') character as the separator
                    string[] VideoPathParts = video.Video.FileName.Split('\\');
                    // Extract the Videoname without the extension (assuming the last part is the Videoname)
                    string VideoNameWithoutExtension = VideoPathParts[VideoPathParts.Length - 1];

                    string name = VideoNameWithoutExtension.Split(".")[0];
                    string VideoName = name + "_" + com.Id + ".ts";
                    DateTime from = convert(long.Parse(name.Split("_")[0]));
                    DateTime to = convert(long.Parse(name.Split("_")[1]));
                    string folder = from.Date.ToString("ddMMyyyy");
                    if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "file", folder)))
                    {
                        // Nếu không tồn tại, tạo thư mục mới
                        Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "file", folder));
                    }
                    if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "file", folder, com.Id.ToString())))
                    {
                        // Nếu không tồn tại, tạo thư mục mới
                        Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "file", folder, com.Id.ToString()));
                    }
                    string fPath = Path.Combine(_env.ContentRootPath, "file", folder, com.Id.ToString(), VideoName); // Or use your preferred storage location
                    using (var stream = new FileStream(fPath, FileMode.Create))
                    {
                        await video.Video.CopyToAsync(stream);

                    }
                    Videos videos = new Videos()
                    {
                        VideoPath = Path.Combine("file", folder, com.Id.ToString(), VideoName),
                        //Keylog = video.keylog,
                        //Apps = video.apps,
                        ChannelId = com.Id,
                        CreatedDate = DateTime.Now,
                        Year = from.Year,
                        Month = from.Month,
                        Date = from.Day,
                        Hours = from.Hour,
                        Minutes = from.Minute,
                        Start = from,
                        End = to,
                        IsDelete = 0
                    };
                    db.Videos.Add(videos);
                    db.SaveChanges();
                    List<UserSession> userSessions = readUserSession(video);
                    foreach (var item in userSessions)
                    {
                        item.VideoId = videos.Id;
                    }
                    db.AddRange(userSessions);
                    db.SaveChanges();
                    List<UserAction> userActions = readUserAction(video);
                    foreach (var item in userActions)
                    {
                        item.VideoId = videos.Id;
                    }
                    db.AddRange(userActions);
                    db.SaveChanges();

                    _logger.LogInformation("Tạo mới video: " + VideoName);
                }


                return Ok("Videos uploaded successfully");
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
        private List<UserSession> readUserSession(VideoDto video)
        {
            string fPath = Path.Combine(_env.ContentRootPath, "usersession_"+video.token + DateTime.Now.Ticks + ".json" ); // Or use your preferred storage location
            using (var stream = new FileStream(fPath, FileMode.Create))
            {
                video.UserSession.CopyToAsync(stream);
            }
            string jsonData = UtilHelper.ReadJsonFile(fPath);
            UtilHelper.Delete(fPath);
            // Deserialize the JSON string into a list of WindowData objects
            return JsonConvert.DeserializeObject<List<UserSession>>(jsonData);
        }
        private List<UserAction> readUserAction(VideoDto video)
        {
            string fPath = Path.Combine(_env.ContentRootPath, "useraction_" + video.token + DateTime.Now.Ticks + ".json"); // Or use your preferred storage location
            using (var stream = new FileStream(fPath, FileMode.Create))
            {
                video.UserAction.CopyToAsync(stream);
            }
            string jsonData = UtilHelper.ReadJsonFile(fPath);
            UtilHelper.Delete(fPath);
            // Deserialize the JSON string into a list of WindowData objects
            return JsonConvert.DeserializeObject<List<UserAction>>(jsonData);
        }
        private static DateTime convert (long ticks)
        {
            var when = new DateTime(1970, 1, 1).AddSeconds(ticks);

            // Convert the UTC DateTime to the local time zone (Hanoi, Vietnam)
            DateTime localDateTime = when.ToLocalTime();
            return localDateTime;
        }
        private static long ConvertToTicks(DateTime localDateTime)
        {
            // Xác định múi giờ địa phương của Hà Nội, Việt Nam
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển đổi thời gian từ giờ địa phương sang giờ UTC
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZoneInfo);

            // Tính số giây từ Unix epoch (1/1/1970) đến thời điểm UTC
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long ticks = (long)(utcDateTime - unixEpoch).TotalSeconds;

            return ticks;
        }
    }
}
