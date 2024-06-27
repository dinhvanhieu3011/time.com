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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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


namespace BookingApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
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

        public UploadController(IComputerRepository computerRepository, IUsersDTRepository usersDTRepository,
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

        [HttpPost]
        public JsonResponse CreateMessage(string ChatId, long Timestamp, string FromPhoneNumber,
            string FromId,string ToPhoneNumber,string ToId, string Message)
        {
            try
            {
                    WhatsAppChat channel = new WhatsAppChat();
                    channel.Id = Guid.NewGuid().ToString();
                    channel.ChatId = ChatId;
                    channel.Timestamp = Timestamp;
                    channel.FromPhoneNumber = FromPhoneNumber;
                    channel.FromId = FromId;
                    channel.ToPhoneNumber = ToPhoneNumber;
                    channel.ToId = ToId;
                    channel.Message = Message;
                    channel.Time = UnixTimeStampToDateTime(Timestamp);

                      _whatsAppChatRepository.Insert(channel);
                    _unitOfWork.Complete();
                    this.response.Success = true;
                    return this.response;
                

            }
            catch
            {
                this.response.Success = false;

                return this.response;
            }

        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private bool? Insert(string ComputerName, string Token, string EmployeeName,string Version,string LinkLive)
        {
            try
            {
                var computer = _computerRepository.GetAll().Where(x => x.Token == Token).FirstOrDefault();
                if (computer == null)
                {
                    ChannelYoutubes channel = new ChannelYoutubes();
                    channel.Name = ComputerName;
                    channel.Token = Token;
                    channel.EmployeeName = EmployeeName;
					channel.Version = Version;
                    channel.LinkLive = LinkLive;
                    channel.ModifiedDate = DateTime.Now;
                    channel.Status = "1";
                    _computerRepository.Insert(channel);
                    _unitOfWork.Complete();
                    return true;
                }
                else
                {
                    computer.Name = ComputerName;
                    computer.Token = Token;
                    computer.EmployeeName = EmployeeName;
                    computer.Version = Version;
                    computer.LinkLive = LinkLive;
                    computer.ModifiedDate = DateTime.Now;
                    computer.Status = "1";

                    _computerRepository.Update(computer);
                    _unitOfWork.Complete();
                    return true;

                }

            }
            catch
            {
                return false;
            }
        }
        private readonly JsonResponse response = new JsonResponse();
        [HttpPost]
        public JsonResponse CreateComputer(string ComputerName, string Token, string EmployeeName,string Version, string LinkLive)
        {
            if (!ModelState.IsValid)
            {
                this.response.Success = false; 
            }
            switch (Insert(ComputerName, Token,EmployeeName, Version, LinkLive))
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
                CultureInfo provider = CultureInfo.CurrentCulture;
                DateTime dateTime = DateTime.Now;
                if (!string.IsNullOrEmpty(Ngay))
                {
                    dateTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider);
                }

                var query = from session in _userSessionRepository.GetAll().OrderBy(s => s.StartTime)
                            join video in _videosRepository.GetAll() on session.VideoId equals video.Id
                            join channel in _computerRepository.GetAll() on video.ChannelId equals channel.Id
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
        public JsonResponse getListDataVideo(string KeyWord, string id, int pageIndex, int pageSize)
        {
            try
            {
                var totalRow = 0;
                if(!string.IsNullOrEmpty(KeyWord))
                    KeyWord = KeyWord.ToLower();
                var listUserActions = _userActionRepository.GetAll()
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
        public WhatsAppChatModel getChat(string chatId, string myNum)
        {


            var listChat = _whatsAppChatRepository.GetAll().Where(x => x.ChatId == chatId).ToList();
            var data = new WhatsAppChatModel
            {
                Key = chatId,
                PhoneNumer = listChat.FirstOrDefault().ToPhoneNumber != myNum ? listChat.FirstOrDefault().ToPhoneNumber : listChat.FirstOrDefault().FromPhoneNumber,
                LastMessage = listChat.OrderBy(p => p.Time).FirstOrDefault().Message,
                whatsAppChats = listChat.OrderBy(x => x.Time).ToList()
            };
            return data;
		}
        [HttpGet]
        public JsonResponse getListComputer(string Ngay,  int pageIndex, int pageSize)
        {
            try
            {
                var totalRow = 0;
                List<ChannelYoutubes> data = new List<ChannelYoutubes>();
                CultureInfo provider = CultureInfo.CurrentCulture;
                if (string.IsNullOrEmpty(Ngay))
                {
                    data = _computerRepository.GetAll().Where(x=>x.Status == "1").ToList();
                }
                else
                {
                    data = _computerRepository.GetAll()
                        .Where(x => x.Status == "1" && (x.Name.Contains(Ngay) || x.EmployeeName.Contains(Ngay)))
                        .ToList();
                }
                foreach (var item in data)
                {
                    if(item.ModifiedDate.AddMinutes(3) >= DateTime.Now)
                    {
                        item.Status = "Đang online";
                    }
                    else
                    {
                        item.Status = "Mất kết nối";
                    }
                }

                totalRow = data.Count();
                this.response.Data = data.OrderByDescending(x => x.Id).ToList();
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
        protected async void sendmessageTelegram(string message)
        {
            try
            {
                var user = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == "admin");
                string url = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", user.TeleToken, user.ChatId, message);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch { }



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
                    data = _videosRepository.GetAll().Where(x=>(x.ChannelId == id || id == 0) && x.IsDelete == 0).ToList();
                }
                else
                {
                    DateTime dateTime = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider);
                    data = _videosRepository.GetAll().Where(x=> (x.ChannelId == id || id == 0) && x.Start.Date == dateTime && x.IsDelete == 0).ToList();
                }

                totalRow = data.Count();
                this.response.Data = data.OrderByDescending(x => x.Start).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
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
        public Videos Get(string id)
        {

            var book = _videosRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
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
                var computers = _computerRepository.GetAll().ToList();
                var com = computers.Where(x => x.Token == video.token).FirstOrDefault();
                if(com != null)
                {
                    com.LinkLive = video.linkLive;
                    com.ModifiedDate = DateTime.Now;
                    _computerRepository.Update(com);
                    _unitOfWork.Complete();
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
                    if (_videosRepository.GetAll().Where(x => x.VideoPath.Contains(VideoName)).Any())
                    {
                        return Ok("Videos uploaded successfully");
                    }
                    Videos videos = new Videos()
                    {
                        Id = Guid.NewGuid().ToString(),
                        VideoPath = Path.Combine("file", folder, com.Id.ToString(), VideoName),
                        //Keylog = video.keylog,
                        //Apps = video.apps,
                        ChannelId = com.Id,
                        CreatedDate =   DateTime.Now.ToLocalTime(),
                    Year = from.Year,
                        Month = from.Month,
                        Date = from.Day,
                        Hours = from.Hour,
                        Minutes = from.Minute,
                        Start = from,
                        End = to,
                        IsDelete = 0,
                        
                    };
                    _videosRepository.Insert(videos);
                    _unitOfWork.Complete();
                    List<UserSession> userSessions = readUserSession(video);
                    foreach (var item in userSessions)
                    {
                        item.VideoId = videos.Id;
                        item.Id = Guid.NewGuid().ToString();
                    }
                    _userSessionRepository.InsertMulti(userSessions);
                    _unitOfWork.Complete();
                    List<UserAction> userActions = new List<UserAction>();
                    List<UserActionModel>  userActionModels = readUserAction(video);
                    foreach (var item in userActionModels)
                    {
                        userActions.Add(new UserAction
                        {
                           VideoId = videos.Id,
                           Id = Guid.NewGuid().ToString(),
                            Windows = item.Windows,
                            UserName = item.UserName,
                            Keys = item.Keys,
                            Time = convert(item.Time)
                        });
                    }
                    _userActionRepository.InsertMulti(userActions);
                    _unitOfWork.Complete();
                    _logger.LogInformation("Tạo mới video: " + VideoName);
                    return Ok("Videos uploaded successfully");
                }
                return Ok("Fails");
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
        private List<UserActionModel> readUserAction(VideoDto video)
        {
            string fPath = Path.Combine(_env.ContentRootPath, "useraction_" + video.token + DateTime.Now.Ticks + ".json"); // Or use your preferred storage location
            using (var stream = new FileStream(fPath, FileMode.Create))
            {
                video.UserAction.CopyToAsync(stream);
            }
            string jsonData = UtilHelper.ReadJsonFile(fPath);
            UtilHelper.Delete(fPath);
            // Deserialize the JSON string into a list of WindowData objects
            return JsonConvert.DeserializeObject<List<UserActionModel>>(jsonData);
        }
        public class UserActionModel
        {
            public string Id { get; set; }
            public int Time { get; set; }
            public string Windows { get; set; }
            public string UserName { get; set; }
            public string Keys { get; set; }
            public string VideoId { get; set; }
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
