using StellumbraSite.Data;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Shared.Model;
using Microsoft.EntityFrameworkCore;
using StellumbraSite.Components.Pages;

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
        [HttpGet("GetAllNews")]
        public async Task<IActionResult> GetAllNews()
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
        [HttpGet("GetNews/{page}/{pageSize}")]
        public async Task<IActionResult> GetNews(int page, int pageSize)
        {
            try
            {
                var result = await _db.NewsItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new NewsItem
                {
                    Id = x.Id,
                    PostId = x.PostId,
                    Title = x.Title,
                    TitleImagePath = x.TitleImagePath,
                    Caption = x.Caption,
                    DateTime = x.DateTime
                })
                .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpGet("GetLatestNews/{articleCount}")]
        public async Task<IActionResult> GetLatestNews(int articleCount)
        {
            try
            {
                int newsCount = await _db.NewsItems.CountAsync();
                articleCount = Math.Min(articleCount, newsCount);

                var result = await _db.NewsItems
                .Skip(Math.Max(newsCount - articleCount, 0))
                .Take(articleCount)
                .Select(x => new NewsItem
                {
                    Id = x.Id,
                    PostId = x.PostId,
                    Title = x.Title,
                    TitleImagePath = x.TitleImagePath,
                    Caption = x.Caption,
                    DateTime = x.DateTime
                })
                .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpGet("GetNewsByID/{newsID}")]
        public async Task<IActionResult> GetPostByID(string newsID)
        {
            try
            {
                var result = await _db.NewsItems
                .Where(x => x.Id == newsID)
                .Select(x => new NewsItem
                {
                    Id = x.Id,
                    PostId = x.PostId,
                    Title = x.Title,
                    TitleImagePath = x.TitleImagePath,
                    Caption = x.Caption,
                    DateTime = x.DateTime,
                })
                .ToListAsync();
                try
                {
                    return Ok(result[0]);
                }
                catch
                {
                    throw new IndexOutOfRangeException($"A post does not exist whose ID is {newsID}");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpGet("SubmitNews")]
        public async Task<IActionResult> SubmitNews([FromBody] NewsItem newsItem)
        {
            await _db.NewsItems.AddAsync(newsItem);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
