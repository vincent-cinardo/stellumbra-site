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
                    PosterID = x.PosterID,
                    Title = x.Title,
                    DateTime = x.DateTime,
                })
                .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpGet("GetPostByID/{postID}")]
        public async Task<IActionResult> GetPostByID(int postID)
        {
            try
            {
                var result = await _db.ForumPosts
                .Where(x => x.Id == postID)
                .Select(x => new ForumPost
                {
                    Id = x.Id,
                    TopicName = x.TopicName,
                    PosterID = x.PosterID,
                    Title = x.Title,
                    DateTime = x.DateTime,
                })
                .ToListAsync();
                try
                {
                    return Ok(result[0]);
                }
                catch
                {
                    throw new IndexOutOfRangeException($"A post does not exist whose ID is {postID}");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpPost("SubmitPost")]
        public async Task<IActionResult> SubmitPost([FromBody] ForumPost forumPost)
        {
            await _db.ForumPosts.AddAsync(forumPost);
            await _db.SaveChangesAsync();
            return Ok(forumPost);
        }
        [HttpGet("DeletePost/{Id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var item = await _db.ForumPosts.FindAsync(id);
            if (item != null)
            {
                _db.ForumPosts.Remove(item);
                await _db.SaveChangesAsync();
                return Ok();
            }
            return StatusCode(500, $"Internal server error: The post whose ID is {id} did not exist.");
        }
    }
}