using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BookingApp.Controllers;
using BASE.Entity.DexTrack;

namespace BookingApp.Service
{
    public interface ISchedulerService
    {
        Task AutoTrecking();
        Task Backup();
        Task CheckConnection();
    }

    public class SchedulerService : ISchedulerService
    {
        private readonly ILogger _logger;
        private readonly HangfireSetting _hangfireOption;
        private readonly IHostEnvironment _env;
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IComputerRepository _computerRepository;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUserActionRepository _userActionRepository;
        private readonly IVideosRepository _videosRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SchedulerService(ILogger<SchedulerService> logger,
            HangfireSetting hangfireOption, IHostEnvironment env, IHttpContextAccessor httpContextAccessor, 
            IComputerRepository computerRepository, IUsersDTRepository usersDTRepository,
            IUserSessionRepository userSessionRepository, IUserActionRepository userActionRepository,
            IVideosRepository videosRepository,   
             IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _hangfireOption = hangfireOption;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _computerRepository = computerRepository;
            _usersDTRepository = usersDTRepository;
            _userSessionRepository = userSessionRepository;
            _userActionRepository = userActionRepository;
            _videosRepository = videosRepository;
            _unitOfWork = unitOfWork;

        }
        #region merge video
        public async Task AutoTrecking()
        {
            await RunJob();
        }
        public async Task RunJob()
        {
            var OneHoursAgo = DateTime.Now.AddHours(-0.5);
            var listComputer = _computerRepository.GetAll().Where(x => x.Status == "1").Select(x => x.Id).ToList();
            foreach (var item in listComputer)
            {
                var lstVideo = _videosRepository.GetAll()
                    .Where(x => x.IsDelete == 0 && x.ChannelId == item && x.IsMerge == 0 && x.Start < OneHoursAgo).OrderBy(x => x.Id)
                    .GroupBy(p => new { p.Year, p.Month, p.Date, p.Hours })
                    .Select(g => new
                    {
                        Key = g.Key,
                        Videos = g.Where(x => x.Year == g.Key.Year &&
                        x.Year == g.Key.Year &&
                        x.Month == g.Key.Month &&
                        x.Date == g.Key.Date &&
                        x.Hours == g.Key.Hours &&
                        x.IsDelete == 0 && x.ChannelId == item && x.IsMerge == 0
                        ).OrderBy(x => x.Start).ToList()
                    }).ToList();
                foreach (var a in lstVideo)
                {
                    var videos = a.Videos;
                    string fullPath = "";
                    if (videos.Count > 1)
                    {
                        var firstVideo = videos.FirstOrDefault();
                        var lastVideo = videos.LastOrDefault();
                        var videoMergeName = ConvertToTicks(firstVideo.Start) + "_" + ConvertToTicks(lastVideo.End) + "_" + item;
                        var isExist = _videosRepository.GetAll().Where(x=>x.VideoPath.Contains(videoMergeName)).Any();
                        if (!isExist)
                        {
							try
							{
								var list = videos.Select(x => x.VideoPath).ToList();

								fullPath = Helper.CreateMasterM3U8(_env.ContentRootPath, list, videoMergeName + ".m3u8");
							}
							catch (Exception ex)
							{
								_logger.LogError(ex.ToString());
								return;
							}


							_logger.LogInformation("Tạo mới video: " + videoMergeName);
							var video = new Videos()
							{
								Id = Guid.NewGuid().ToString(),
								VideoPath = fullPath,
								Keylog = "",
								Apps = "",
								ChannelId = item,
								CreatedDate = DateTime.Now,
								Year = a.Key.Year,
								Month = a.Key.Month,
								Date = a.Key.Date,
								Hours = a.Key.Hours,
								Minutes = 0,
								Start = firstVideo.Start,
								End = lastVideo.End,
								IsDelete = 0,
								IsMerge = 1
							};

							_videosRepository.Insert(video);
							_unitOfWork.Complete();
							MergeUserSession(videos, video.Id);
							MergeUserAction(videos, video.Id);
							UpdateStatusVideo(videos);
						}
                    }

                }
            }

        }


        private void MergeUserSession(List<Videos> listVideo, string id)
        {
            var videoIds = listVideo.Select(x => x.Id).ToArray();
            var sessions = _userSessionRepository.GetAll().Where(x => videoIds.Contains(x.VideoId)).ToList();
            foreach (var session in sessions)
            {
                session.VideoId = id;
            }
            _userSessionRepository.UpdateMulti(sessions);
            _unitOfWork.Complete();
        }


        private void MergeUserAction(List<Videos> listVideo, string id)
        {
            var videoIds = listVideo.Select(x => x.Id).ToArray();
            var sessions = _userSessionRepository.GetAll().Where(x => videoIds.Contains(x.VideoId)).ToList();
            foreach (var session in sessions)
            {
                session.VideoId = id;
            }
            _userSessionRepository.UpdateMulti(sessions);
            _unitOfWork.Complete();
        }

        private static string MergeFile(string videoMergeName, List<Videos> listVideo, string rootPath)
        {
            var ffmpegPath = Path.Combine(rootPath, "ffmpeg.exe");
            ffmpegPath = "ffmpeg";
            var concatFilePath = Path.Combine(rootPath, "concat.txt");
            string folder = listVideo[0].Start.Date.ToString("ddMMyyyy");
            if (!Directory.Exists(Path.Combine(rootPath, "file", folder)))
            {
                // Nếu không tồn tại, tạo thư mục mới
                Directory.CreateDirectory(Path.Combine(rootPath, "file", folder));
            }
            if (!Directory.Exists(Path.Combine(rootPath, "file", folder, listVideo[0].ChannelId.ToString())))
            {
                // Nếu không tồn tại, tạo thư mục mới
                Directory.CreateDirectory(Path.Combine(rootPath, "file", folder, listVideo[0].ChannelId.ToString()));
            }
            string fPath = Path.Combine(rootPath, "file", folder, listVideo[0].ChannelId.ToString(), videoMergeName); // Or use your preferred storage location
            CreateConcatFile(rootPath, concatFilePath, listVideo);
            MergeVideosWithFFmpeg(ffmpegPath, concatFilePath, fPath + ".mp4");
            return Path.Combine( "file", folder, listVideo[0].ChannelId.ToString(), videoMergeName);
        }
        private static void CreateConcatFile(string rootPath, string filePath, List<Videos> videoFiles)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var videoFile in videoFiles)
                {
                    writer.WriteLine($"file '{Path.Combine(rootPath, videoFile.VideoPath.Replace("/", "\\"))}'");
                }
            }
        }
        private static void MergeVideosWithFFmpeg(string ffmpegPath, string concatFilePath, string outputFilePath)
        {
            // Specify the directory where you want to create the batch file
            string batchFileDirectory = System.IO.Directory.GetCurrentDirectory(); // Change this to the desired directory
            // Create the batch file path
            string batchFilePath = Path.Combine(batchFileDirectory, "runscript.bat");

            // Create the batch file content
            string batchFileContent = $"";
            batchFileContent += $"ffmpeg -f concat -safe 0 -i \"{concatFilePath}\" -c:v libx264 -c:a aac \"{outputFilePath}\"";
            batchFileContent += $" "; // Optional: pause at the end to keep the command window open

            // Write the content to the batch file
            File.WriteAllText(batchFilePath, batchFileContent);
            RunBatFile();
        }
        public static void RunBatFile()
        {


            string batchFileDirectory = System.IO.Directory.GetCurrentDirectory(); // Change this to the desired directory
            string batchFilePath = Path.Combine(batchFileDirectory, "runscript.bat");

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.Verb = "runas";
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.Arguments = "/c " + batchFilePath;


            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();  // Chờ cho đến khi quá trình FFmpeg kết thúc

                if (process.ExitCode != 0)
                {
                    Console.WriteLine("FFmpeg process exited with error.");
                }
                else
                {
                    Console.WriteLine("FFmpeg process completed successfully.");
                }
            }
        }
        public static void CreateBatFile(string username, string ms_token)
        {
            string fileName = "user_example.py"; // Example file name
            string arguments = username + " " + ms_token; // Example arguments

            // Specify the directory where you want to create the batch file
            string batchFileDirectory = System.IO.Directory.GetCurrentDirectory(); // Change this to the desired directory
            // Create the batch file path
            string batchFilePath = Path.Combine(batchFileDirectory, "runscript.bat");

            // Create the batch file content
            string batchFileContent = $"";
            batchFileContent += $"c:\\website\\BookingApp.runtimeconfig\\.venv\\Scripts\\python.exe {fileName} {arguments}";
            batchFileContent += $" "; // Optional: pause at the end to keep the command window open

            // Write the content to the batch file
            File.WriteAllText(batchFilePath, batchFileContent);

        }
        private long ConvertToTicks(DateTime localDateTime)
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
        private string CreateKeyLogString(List<Videos> videoFiles)
        {
            string result = "";
            foreach (var videoFile in videoFiles)
            {
                result = result + videoFile.Start.ToString() + ":" + videoFile.Keylog + " \n";
            }
            return result;
        }
        private string CreateAppsString(List<Videos> videoFiles)
        {
            string result = "";
            foreach (var videoFile in videoFiles)
            {
                result = result + videoFile.Start.ToString() + ":" + videoFile.Apps + " \n";
            }
            return result;
        }
        private void UpdateStatusVideo(List<Videos> videoFiles)
        {
            try
            {
                foreach (var videoFile in videoFiles)
                {
                    videoFile.IsDelete = 1;
                }
                _videosRepository.UpdateMulti(videoFiles);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

        }
        #endregion
        public async Task Backup()
        {

        }
        public async Task CheckConnection()
        {
            var list =  _computerRepository.GetAll().ToList();

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var lastVideoTime = _videosRepository.GetAll().Where(x => x.ChannelId == item.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    if (lastVideoTime != null)
                    {
                        TimeSpan difference = DateTime.Now - lastVideoTime.Start;

                        // Convert the difference to minutes
                        double differenceInMinutes = difference.TotalMinutes;
                        if (differenceInMinutes > 5)
                        {
                            if (item.Status != "0")
                            {
                                item.Status = "0";
                                _computerRepository.Update(item);
                                _unitOfWork.Complete(); 
                                sendmessageTelegram("Mất kết nối tới máy :" + item.Name + " - " + item.Name);
                            }
                        }
                        else
                        {
                            item.Status = "1";
                            _computerRepository.Update(item);
                            _unitOfWork.Complete();
                        }
                    }
                    else
                    {
                        item.Status = "0";
                        _computerRepository.Update(item);
                        _unitOfWork.Complete();
                    }
                }
            }
        }
        protected async void sendmessageTelegram(string message)
        {
            try
            {
                //var username = _httpContextAccessor.HttpContext.Session.GetString("user");
                //var user = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == username);
                //string url = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", user.TeleToken, user.ChatId, message);
                //var client = new HttpClient();
                //var request = new HttpRequestMessage(HttpMethod.Get, url);
                //var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch { }



        }
    }
}
