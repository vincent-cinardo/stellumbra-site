using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Server.Models;
using Microsoft.EntityFrameworkCore;
using StellumbraSite.Server.Services;

namespace StellumbraSite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PostController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("GetPostCount/{topicName}")]
        public async Task<IActionResult> GetPostCount(string topicName)
        {
            int count = await _db.ForumPosts.CountAsync();
            return Ok(count);
        }
        [HttpGet("GetPosts/{topicName}/{page}/{pageSize}")]
        public async Task<IActionResult> GetPosts(string topicName, int page, int pageSize)
        {
            try
            {
                var result = await _db.ForumPosts
                .Where(x => x.TopicName == topicName)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ForumPost
                {
                    Id = x.Id,
                    TopicName = x.TopicName,
                    Title = x.Title
                })
                .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}