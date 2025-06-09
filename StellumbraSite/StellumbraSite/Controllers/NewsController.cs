using StellumbraSite.Data;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace StellumbraSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNews()
        {
            /*
            var allNews = await _db.NewsItems
                .Select(x => new NewsItem
                {
                    Title = x.Title,
                    TitleImagePath = x.TitleImagePath,
                    Date = x.Date,
                    Caption = x.Caption,
                    Content = x.Content,
                })
                .ToListAsync();*/
            return Ok(await _db.NewsItems.ToListAsync());
        }
    }
}
