using StellumbraSite.Data;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PostController(ApplicationDbContext db)
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