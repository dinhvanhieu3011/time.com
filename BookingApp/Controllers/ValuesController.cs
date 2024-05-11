using BMBSOFT.GIS.Infrastructure.Interface;
using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BMBSOFT.GIS.Infrastructure.Implements;
using BMBSOFT.GIS.Infrastructure.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public IPagedList<News> Get1()
        {
            int PageIndex = 0; int PageSize = 10;
            using var db = new AppDbContext();
            var listNews = db.News.OrderByDescending(x => x.CreatedDate).ToList();
            if (PageIndex == 0) PageIndex = 1;
            if (PageSize == 0) PageSize = 10;

            var response = listNews?
                    .AsEnumerable()
                    //.Select(x => _mapper.Map<NewsDto>(x))
                    .AsQueryable()
                    .OrderByDescending(x => x.CreatedDate)
                    .ToPagedList(PageIndex, PageSize);
            return response;
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
