using StellumbraSite.Data;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ThreadController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet("GetThreadCount/{topicName}")]
        public async Task<IActionResult> GetPostCount(string topicName)
        {
            int count = await _db.ForumThreads.CountAsync();
            return Ok(count);
        }
        [HttpGet("GetThreadPostCount/{userID}")]
        public async Task<IActionResult> GetUserPostCount(string userID)
        {
            int count = await _db.ForumThreads.CountAsync(x => x.PosterID == userID);
            return Ok(count);
        }
        [HttpGet("GetThreads/{topicName}/{page}/{pageSize}")]
        public async Task<IActionResult> GetThreads(string topicName, int page, int pageSize)
        {
            try
            {
                var result = await _db.ForumThreads
                .Where(x => x.TopicName == topicName)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ForumThread
                {
                    Id = x.Id,
                    TopicName = x.TopicName,
                    PosterID = x.PosterID,
                    Title = x.Title,
                    Views = x.Views,
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
        [HttpGet("GetRecentThreads/{limit}")]
        public async Task<IActionResult> GetRecentThreads(int limit)
        {
            try
            {
                var result = await _db.ForumThreads
                .Skip(Math.Max(await _db.ForumThreads.CountAsync() - limit, 0))
                .Select(x => new ForumThread
                {
                    Id = x.Id,
                    TopicName = x.TopicName,
                    PosterID = x.PosterID,
                    Title = x.Title,
                    Views = x.Views,
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
        [HttpGet("GetThreadByID/{postID}")]
        public async Task<IActionResult> GetThreadByID(int postID)
        {
            try
            {
                var result = await _db.ForumThreads
                .Where(x => x.Id == postID)
                .Select(x => new ForumThread
                {
                    Id = x.Id,
                    TopicName = x.TopicName,
                    PosterID = x.PosterID,
                    Title = x.Title,
                    Views = x.Views,
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
        [HttpPost("SubmitThread")]
        public async Task<IActionResult> SubmitThread([FromBody] ForumThread forumThread)
        {
            await _db.ForumThreads.AddAsync(forumThread);
            await _db.SaveChangesAsync();
            return Ok(forumThread);
        }
        [HttpGet("GetReplies/{Id}")]
        public async Task<IActionResult> GetReplies(int id)
        {
            var item = await _db.ForumPosts.FindAsync(id);
            if (item != null)
            {
                int replyCount = await _db.ForumPosts.CountAsync(x => x.ThreadID == id) - 1;
                return Ok(replyCount);
            }
            return StatusCode(500, $"Internal server error: The post whose ID is {id} did not exist.");
        }
        [HttpGet("GetViews/{Id}")]
        public async Task<IActionResult> GetViews(int id)
        {
            var item = await _db.ForumThreads.FindAsync(id);
            if (item != null)
            {
                return Ok(item.Views);
            }
            return StatusCode(500, $"Internal server error: The post whose ID is {id} did not exist.");
        }
        [HttpGet("AddView/{Id}")]
        public async Task<IActionResult> AddView(int id)
        {
            var post = await _db.ForumThreads.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Views += 1;

            await _db.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("DeleteThread/{Id}")]
        public async Task<IActionResult> DeleteThread(int id)
        {
            var item = await _db.ForumThreads.FindAsync(id);
            if (item != null)
            {
                _db.ForumThreads.Remove(item);
                await _db.SaveChangesAsync();
                return Ok();
            }
            return StatusCode(500, $"Internal server error: The post whose ID is {id} did not exist.");
        }
    }
}