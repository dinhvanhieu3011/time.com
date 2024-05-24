using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookingApp.Data;
using System.Collections.Generic;
using System.Text.Json;
using BookingApp.DB.Classes.DB;
using System.Linq;
using System.Threading.Channels;
using Hangfire;
using System.Reflection;
using Hangfire.Storage;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingApp.Service
{
    public interface ISchedulerService
    {
        Task AutoTrecking();
        Task Backup();
    }

    public class SchedulerService : ISchedulerService
    {
        private readonly ILogger _logger;
        private readonly HangfireSetting _hangfireOption;
        private readonly IHostEnvironment _env;

        public SchedulerService(ILogger<SchedulerService> logger,
            HangfireSetting hangfireOption, IHostEnvironment env)
        {
            _logger = logger;
            _hangfireOption = hangfireOption;
            _env = env;
        }
        #region merge video
        public async Task AutoTrecking()
        {
            await RunJob();
        }

        public async Task RunJob()
        {
            var db = new AppDbContext();
            var OneHoursAgo = DateTime.Now.AddHours(-2);
            var listComputer = db.Videos.Select(x=>x.ChannelId).Distinct().ToList(); 
            foreach (var item in listComputer)
            {
                // lấy tất cả video của 1 tiếng trước
                var listVideo = db.Videos.Where(x => x.IsDelete == 0 && x.ChannelId == item
                && x.Year == OneHoursAgo.Year && x.Month == OneHoursAgo.Month
                && x.Date == OneHoursAgo.Day && x.Hours == OneHoursAgo.Hour
                && x.IsMerge != 1).OrderBy(x => x.Id).ToList();

                if (listVideo.Count > 1)
                {
                    var firstVideo = listVideo.FirstOrDefault();
                    var lastVideo = listVideo.LastOrDefault();
                    var videoMergeName = ConvertToTicks(firstVideo.Start) + "_" + ConvertToTicks(lastVideo.End) + "_" + item;
                    try
                    {
                        MergeFile(videoMergeName, listVideo, _env.ContentRootPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        return;
                    }

                    //1. Create bản ghi mới
               

                    string keyLogString = CreateKeyLogString(listVideo);
                    string appString = CreateAppsString(listVideo);
                    _logger.LogInformation("Tạo mới video: " + videoMergeName );
                    var video = new Videos()
                    {
                        VideoPath = Path.Combine("file", videoMergeName + ".mp4"),
                        Keylog = keyLogString,
                        Apps = appString,
                        ChannelId = item,
                        CreatedDate = DateTime.Now,
                        Year = OneHoursAgo.Year,
                        Month = OneHoursAgo.Month,
                        Date = OneHoursAgo.Day,
                        Hours = OneHoursAgo.Hour,
                        Minutes = 0,
                        Start = firstVideo.Start,
                        End = lastVideo.End,
                        IsDelete = 0,
                        IsMerge = 1
                    };

                    db.Add(video);
                    db.SaveChanges();
                    UpdateStatusVideo(listVideo);
                }    
            }

        }
        private static void MergeFile(string videoMergeName, List<Videos> listVideo, string rootPath)
        {
            var ffmpegPath = Path.Combine(rootPath, "ffmpeg.exe") ;
            ffmpegPath = "ffmpeg";
            var concatFilePath = Path.Combine(rootPath, "concat.txt") ;
            string fPath = Path.Combine(rootPath, "file", videoMergeName); // Or use your preferred storage location
            CreateConcatFile(rootPath,concatFilePath, listVideo);
            MergeVideosWithFFmpeg(ffmpegPath, concatFilePath, fPath + ".mp4");
        }
        private static void CreateConcatFile(string rootPath, string filePath, List<Videos> videoFiles)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var videoFile in videoFiles)
                {
                    writer.WriteLine($"file '{Path.Combine(rootPath, videoFile.VideoPath.Replace("/","\\"))}'");
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
                var db = new AppDbContext();
                db.UpdateRange(videoFiles);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

        }
        #endregion

        public async Task Backup()
        {
            //string sourceDirectory = @"C:\path\to\your\directory";
            //string destinationDirectory = @"C:\path\to\backup\directory";
            //string databaseFile = @"C:\path\to\your\directory\booking.db";
            //string backupDatabaseFile = Path.Combine(destinationDirectory, "booking.db");

            //try
            //{
            //    // Create backup directory if it doesn't exist
            //    if (!Directory.Exists(destinationDirectory))
            //    {
            //        Directory.CreateDirectory(destinationDirectory);
            //    }

            //    // Copy files from the source directory to the backup directory
            //    foreach (string file in Directory.GetFiles(sourceDirectory))
            //    {
            //        string destFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
            //        File.Copy(file, destFile, true);
            //    }

            //    // Copy the database file
            //    File.Copy(databaseFile, backupDatabaseFile, true);

            //    Console.WriteLine("Backup completed successfully.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error during backup: {ex.Message}");
            //}
        }
    }
}
