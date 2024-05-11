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

namespace BookingApp.Service
{
    public interface ISchedulerService
    {
        Task AutoTrecking();
        DateTime? GetNextExecutionTime();
    }

    public class SchedulerService : ISchedulerService
    {
        private readonly ILogger _logger;
        private readonly HangfireSetting _hangfireOption;

        public SchedulerService(ILogger<SchedulerService> logger,
            HangfireSetting hangfireOption)
        {
            _logger = logger;
            _hangfireOption = hangfireOption;
        }
        public async Task AutoTrecking()
        {
            using var db = new AppDbContext();
            var version = db.Settings.FirstOrDefault().Email;
            if(version == "v2")
            {
                await TreckkingAsync_v2();

            }
            else if (version == "v1")
            {
                await TreckkingAsync();

            }
            else
            {
                await TreckkingAsync_v3();
            }
        }

        public async Task TreckkingAsync_v3()
        {
            using var db = new AppDbContext();
            var profiles = db.Channels.Select(x => x.Name).ToList();
            var api_secret = db.Settings.FirstOrDefault().MailHost;

            foreach (var profile in profiles)
            {
                Helper.Log(profile);
                Helper.Log(api_secret);

                var json = Helper.CallPython(profile);
                var data = JsonConvert.DeserializeObject<VideoFromPython>(json);
                if (data == null)
                {
                    var channel = db.Channels.Where(x => x.Name.ToLower() == profile).FirstOrDefault();

                    channel.Status = 0;
                    channel.Action = 0;// THAY ACC
                    db.Channels.Update(channel);
                    db.SaveChanges();
                }
                else if (data.videos.Count == 0)
                {
                    var channel = db.Channels.Where(x => x.Name.ToLower() == profile).FirstOrDefault();

                    channel.VideoCount = 0;
                    channel.Action = 1;
                    channel.Status = 1;
                    channel.LikeCount = 0;
                    channel.LastVideoCreatedDate = DateTime.Now.AddYears(-2000);
                    db.Channels.Update(channel);
                    db.SaveChanges();
                }
                else
                {
                    var channel = db.Channels.Where(x => x.Name.ToLower() == profile).FirstOrDefault();

                    channel.Status = 1;
                    channel.VideoCount = data.videos.Count;
                    channel.LastVideoCreatedDate = Helper.epoch2string(data.videos.OrderByDescending(x => x.createTime).FirstOrDefault().createTime);
                    channel.ViewCount = data.videos.Sum(x => x.stats.playCount);
                    channel.LikeCount = data.videos.Sum(x => x.stats.diggCount);
                    if (channel.VideoCount > 6)
                    {
                        channel.Action = 2; //DƯ VIDEO
                    }
                    else if (DateTime.Now - channel.LastVideoCreatedDate > TimeSpan.FromHours(2))
                    {
                        channel.Action = 3; //  ACC KẸT
                    }
                    else if (DateTime.Now - channel.LastVideoCreatedDate > TimeSpan.FromMinutes(15) && channel.ViewCount < 100)
                    {
                        channel.Action = 4; // CHECK ACC
                    }
                    else
                    {
                        channel.Action = 1; // GOOD
                    }
                    db.Channels.Update(channel);
                    db.SaveChanges();
                }
            }
        }

        public async Task TreckkingAsync()
        {
            using var db = new AppDbContext();

            var client = new HttpClient();
            var api_secret = db.Settings.FirstOrDefault().MailHost;
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://api.apify.com/v2/acts/clockworks~tiktok-scraper/run-sync-get-dataset-items?token=apify_api_cfYieafk89RpqbBbS3yexX9R21ZAco1W1dO3");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.apify.com/v2/acts/clockworks~tiktok-scraper/run-sync-get-dataset-items?token="+ api_secret + "");
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://api.apify.com/v2/acts/clockworks~tiktok-scraper/run-sync-get-dataset-items?token=apify_api_zQ0sXbyuh28s91KgSnS3GnhB907IIP3h6iFx");
            //var content = new StringContent("{\r\n    \"profiles\": [\r\n        \"k1ms1on\"\r\n    ],\r\n    \"resultsPerPage\": 7,\r\n    \"shouldDownloadCovers\": false,\r\n    \"shouldDownloadSlideshowImages\": false,\r\n    \"shouldDownloadVideos\": false\r\n}", null, "application/json");
            var profiles = db.Channels.Select(x => x.Name).ToList();
            var content = Helper.CreateContent(profiles);

            var content1 = new StringContent("{\r\n    \"profiles\": " + content + ",\r\n    \"resultsPerPage\": 7,\r\n    \"shouldDownloadCovers\": false,\r\n    \"shouldDownloadSlideshowImages\": false,\r\n    \"shouldDownloadVideos\": false\r\n}", null, "application/json");
            request.Content = content1;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string json = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<TreckingData>>(json);

            // List không có video : name
            var lstNoVideo = data.Where(x => !string.IsNullOrEmpty(x.name)).ToList();
            foreach ( var item in lstNoVideo)
            {
                var channel = db.Channels.Where(x => x.Name.ToLower() == item.name.ToLower()).FirstOrDefault();
                channel.VideoCount = 0;
                channel.Action = 1;
                channel.Status = 1;
                channel.LikeCount = 0;
                channel.LastVideoCreatedDate = DateTime.Now.AddYears(-2000);
                db.Channels.Update(channel);
                db.SaveChanges();
            }

            var lstDie = data.Where(x => !string.IsNullOrEmpty(x.error)).ToList();
            foreach (var item in lstDie)
            {
                string name = item.url.Replace("https://www.tiktok.com/@", "");
                var channel = db.Channels.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
                channel.Status = 0;
                channel.Action = 0;// THAY ACC
                db.Channels.Update(channel);
                db.SaveChanges();
            }

            var lstHasVideo = data.Where(x => x.authorMeta!=null).ToList();
            foreach (var item in lstHasVideo)
            {
                    var channel = db.Channels.Where(x => x.Name.ToLower() == item.authorMeta.name.ToLower()).FirstOrDefault();
                    channel.Status = 1;
                    channel.VideoCount = item.authorMeta.video;
                    channel.LastVideoCreatedDate = lstHasVideo.Where(x => x.authorMeta.name.ToLower() == channel.Name.ToLower()).OrderByDescending(x=>x.createTimeISO).FirstOrDefault().createTimeISO;
                    channel.ViewCount = lstHasVideo.Where(x =>  x.authorMeta.name.ToLower() == channel.Name.ToLower()).Sum(x => x.playCount);
                    channel.LikeCount = lstHasVideo.Where(x =>  x.authorMeta.name.ToLower() == channel.Name.ToLower()).Sum(x => x.diggCount);
                    if (channel.VideoCount > 6)
                    {
                        channel.Action = 2; //DƯ VIDEO
                    }
                    else if (DateTime.UtcNow - channel.LastVideoCreatedDate > TimeSpan.FromHours(2))
                    {
                        channel.Action = 3; //  ACC KẸT
                    }
                    else if (DateTime.UtcNow - channel.LastVideoCreatedDate > TimeSpan.FromMinutes(15) && channel.ViewCount < 100)
                    {
                        channel.Action = 4; // CHECK ACC
                    }
                    else
                    {
                        channel.Action = 1; // GOOD
                    }
                    db.Channels.Update(channel);
                    db.SaveChanges();
            }
        }

        public async Task TreckkingAsync_v2()
        {
            using var db = new AppDbContext();

            var api_secret = db.Settings.FirstOrDefault().MailHost;
            var profiles = db.Channels.Select(x => x.Name).ToList();
            foreach (var item in profiles)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://tiktok-api6.p.rapidapi.com/user/videos?username=" + item + ""),
                    Headers =
                                {
                                    { "X-RapidAPI-Key", api_secret },
                                    { "X-RapidAPI-Host", "tiktok-api6.p.rapidapi.com" },
                                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    string json = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Videos>(json);
                    if (!string.IsNullOrEmpty(data.detail))
                    {
                        var channel = db.Channels.Where(x => x.Name.ToLower() == item.ToLower()).FirstOrDefault();
                        channel.Status = 0;
                        channel.Action = 0;// THAY ACC
                        db.Channels.Update(channel);
                        db.SaveChanges();
                    }
                    else if (data.videos.Count == 0)
                    {
                        var channel = db.Channels.Where(x => x.Name.ToLower() == item.ToLower()).FirstOrDefault();
                        channel.VideoCount = 0;
                        channel.Action = 1;
                        channel.Status = 1;
                        channel.LikeCount = 0;
                        channel.LastVideoCreatedDate = DateTime.Now.AddYears(-2000);
                        db.Channels.Update(channel);
                        db.SaveChanges();
                    }
                    else
                    {
                        var channel = db.Channels.Where(x => x.Name.ToLower() == item.ToLower()).FirstOrDefault();
                        channel.Status = 1;
                        channel.VideoCount = data.videos.Count;
                        channel.LastVideoCreatedDate = Helper.epoch2string(data.videos.OrderByDescending(x => x.create_time).FirstOrDefault().create_time);
                        channel.ViewCount = data.videos.Sum(x => x.statistics.number_of_plays);
                        channel.LikeCount = data.videos.Sum(x => x.statistics.number_of_hearts);
                        if (channel.VideoCount > 6)
                        {
                            channel.Action = 2; //DƯ VIDEO
                        }
                        else if (DateTime.UtcNow - channel.LastVideoCreatedDate > TimeSpan.FromHours(2))
                        {
                            channel.Action = 3; //  ACC KẸT
                        }
                        else if (DateTime.UtcNow - channel.LastVideoCreatedDate > TimeSpan.FromMinutes(15) && channel.ViewCount < 100)
                        {
                            channel.Action = 4; // CHECK ACC
                        }
                        else
                        {
                            channel.Action = 1; // GOOD
                        }
                        db.Channels.Update(channel);
                        db.SaveChanges();
                    }
                }
            }

        }
        public DateTime? GetNextExecutionTime()
        {
            try
            {
                var job = JobStorage.Current.GetConnection().GetRecurringJobs().FirstOrDefault();
                return job == null ? null : job.NextExecution;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
