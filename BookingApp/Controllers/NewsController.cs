using BMBSOFT.GIS.Infrastructure.Interface;
using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BMBSOFT.GIS.Infrastructure.Implements;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        // GET: api/<NewsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public List<NewsModel> GetHot()
        {
            using var db = new AppDbContext();
            var listNews = db.Files.Where(x=>x.Status!= "Duration > 20 mins").OrderByDescending(x => x.CreatedDate).Take(5).ToList();

            var response = listNews?
                    .AsEnumerable()
                    .Select(x => new NewsModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Sumary = x.FilePath.Substring(0, 20) + "...",
                        Description = x.FilePath,
                        Image = "/file/" + x.VideoPath + ".webp",
                        YoutbeLink = x.UrlVideo,
                        Author = x.ChannelYoutubeName.Replace("https://www.youtube.com/@", "").Replace("/videos", ""),
                        created_date = x.CreatedDate.ToShortDateString(),
                    })
                    .AsQueryable().ToList();
            return response;
        }
        [HttpGet()]
        public List<NewsModel> GetRecomend()
        {
            using var db = new AppDbContext();
            var listNews = db.Files.Where(x => x.Status != "Duration > 20 mins").OrderByDescending(x => x.CreatedDate).ToList();
            var response = listNews?
                    .AsEnumerable()
                    .Select(x => new NewsModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Sumary = x.FilePath.Substring(0, 20) + "...",
                        Description = x.FilePath,
                        Image = "/file/" + x.VideoPath + ".webp",
                        YoutbeLink = x.UrlVideo,
                        Author = x.ChannelYoutubeName.Replace("https://www.youtube.com/@", "").Replace("/videos", ""),
                        created_date = x.CreatedDate.ToShortDateString(),
                    })
                    .AsQueryable()
                   
                    .Take(5)
                    .ToList();
            return response;
        }
        [HttpGet("{id}")]
        public IPagedList<NewsModel> Get(int id)
        {
            int PageIndex = id; int PageSize = 10;
            using var db = new AppDbContext();
            var listNews = db.Files.Where(x => x.Status != "Duration > 20 mins").OrderByDescending(x => x.CreatedDate).ToList();
            if (PageIndex == 0) PageIndex = 1;
            if (PageSize == 0) PageSize = 10;

            var response = listNews?
                    .AsEnumerable()
                    .Select(x => new NewsModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Sumary = x.FilePath.Substring(0,20) + "...",
                        Description = x.FilePath,
                        Image = "/file/" + x.VideoPath + ".webp",
                        YoutbeLink = x.UrlVideo,
                        Author = x.ChannelYoutubeName.Replace("https://www.youtube.com/@", "").Replace("/videos",""),
                        created_date = x.CreatedDate.ToShortDateString(),
                    })
                    .AsQueryable()
                    .ToPagedList(PageIndex, PageSize);
            return response;
        }
        [HttpGet("{id}")]
        public NewsModel GetDetail(int id)
        {
            try
            {
                int PageIndex = id; int PageSize = 10;
                using var db = new AppDbContext();
                var x = db.Files.Where(x => x.Status != "Duration > 20 mins").Where(x => x.Id == id).FirstOrDefault();
                if (PageIndex == 0) PageIndex = 1;
                if (PageSize == 0) PageSize = 10;

                var response = new NewsModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Sumary = x.FilePath.Substring(0, 100) + "...",
                    Description = x.FilePath,
                    Image = "/file/" + x.VideoPath + ".webp",
                    YoutbeLink = x.UrlVideo,
                    Author = x.ChannelYoutubeName.Replace("https://www.youtube.com/@", "").Replace("/videos", ""),
                    created_date = x.CreatedDate.ToShortDateString(),
                };
                return response;
            }
            catch { return null; }


        }

        public class NewsModel()
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Sumary { set; get; }    
            public string Image { get; set; }
            public string Description { get; set; }
            public string created_date { get; set; }
            public string Author { get; set; }
            public string YoutbeLink { get; set; }
        }
        // POST api/<NewsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
