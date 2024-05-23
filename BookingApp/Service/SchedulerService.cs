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
            await TreckkingAsync_v3();
        }

        public async Task TreckkingAsync_v3()
        {
            //using var db = new AppDbContext();
            //var profiles = db.Channels.Select(x => x.Name).ToList();
            //var api_secret = db.Settings.FirstOrDefault().MailHost;

            //foreach (var profile in profiles)
            //{
            //    Helper.Log(profile);
            //    Helper.Log(api_secret);

            //    var json = Helper.CallPython(profile);
            //    var data = JsonConvert.DeserializeObject<VideoFromPython>(json);
            //    if (data == null)
            //    {
            //        var channel = db.Channels.Where(x => x.Name.ToLower() == profile).FirstOrDefault();

            //        channel.Status = 0;
            //        channel.Action = 0;// THAY ACC
            //        db.Channels.Update(channel);
            //        db.SaveChanges();
            //    }
            //    else if (data.videos.Count == 0)
            //    {
            //        var channel = db.Channels.Where(x => x.Name.ToLower() == profile).FirstOrDefault();

            //        channel.VideoCount = 0;
            //        channel.Action = 1;
            //        channel.Status = 1;
            //        channel.LikeCount = 0;
            //        channel.LastVideoCreatedDate = DateTime.Now.AddYears(-2000);
            //        db.Channels.Update(channel);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        var channel = db.Channels.Where(x => x.Name.ToLower() == profile).FirstOrDefault();

            //        channel.Status = 1;
            //        channel.VideoCount = data.videos.Count;
            //        channel.LastVideoCreatedDate = Helper.epoch2string(data.videos.OrderByDescending(x => x.createTime).FirstOrDefault().createTime);
            //        channel.ViewCount = data.videos.Sum(x => x.stats.playCount);
            //        channel.LikeCount = data.videos.Sum(x => x.stats.diggCount);
            //        if (channel.VideoCount > 6)
            //        {
            //            channel.Action = 2; //DƯ VIDEO
            //        }
            //        else if (DateTime.Now - channel.LastVideoCreatedDate > TimeSpan.FromHours(2))
            //        {
            //            channel.Action = 3; //  ACC KẸT
            //        }
            //        else if (DateTime.Now - channel.LastVideoCreatedDate > TimeSpan.FromMinutes(15) && channel.ViewCount < 100)
            //        {
            //            channel.Action = 4; // CHECK ACC
            //        }
            //        else
            //        {
            //            channel.Action = 1; // GOOD
            //        }
            //        db.Channels.Update(channel);
            //        db.SaveChanges();
            //    }
            //}
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
