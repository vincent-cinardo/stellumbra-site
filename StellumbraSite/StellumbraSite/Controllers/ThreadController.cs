using StellumbraSite.Data;
using StellumbraSite.Model;
using Microsoft.AspNetCore.Mvc;
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
            int count = await _db.ForumThreads.CountAsync(x => x.PosterId == userID);
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
                    PosterId = x.PosterId,
                    Title = x.Title,
                    Views = x.Views,
                    DateTime = x.DateTime,
                    Topic = x.Topic,
                    ApplicationUser = x.ApplicationUser,
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
                    PosterId = x.PosterId,
                    Title = x.Title,
                    Views = x.Views,
                    DateTime = x.DateTime,
                    Topic = x.Topic,
                    ApplicationUser = x.ApplicationUser,
                })
                .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpGet("GetThreadByID/{threadID}")]
        public async Task<IActionResult> GetThreadByID(int threadID)
        {
            try
            {
                var result = await _db.ForumThreads
                .Where(x => x.Id == threadID)
                .Select(x => new ForumThread
                {
                    Id = x.Id,
                    TopicName = x.TopicName,
                    PosterId = x.PosterId,
                    Title = x.Title,
                    Views = x.Views,
                    DateTime = x.DateTime,
                    Topic = x.Topic,
                    ApplicationUser = x.ApplicationUser,
                })
                .ToListAsync();
                try
                {
                    return Ok(result[0]);
                }
                catch
                {
                    throw new IndexOutOfRangeException($"A post does not exist whose ID is {threadID}");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpPost("SubmitThread")]
        public async Task<IActionResult> SubmitThread([FromBody] ForumThreadDto forumThreadDto)
        {
            ForumThread forumThread = new ForumThread();
            forumThread.Id = forumThreadDto.Id;
            forumThread.TopicName = forumThreadDto.TopicName;
            forumThread.PosterId = forumThreadDto.PosterId;
            forumThread.Title = forumThreadDto.Title;
            forumThread.Views = forumThreadDto.Views;

            await _db.ForumThreads.AddAsync(forumThread);
            await _db.SaveChangesAsync();
            return Ok(forumThread);
        }
        [HttpGet("GetReplies/{Id}")]
        public async Task<IActionResult> GetReplies(int id)
        {
            var item = await _db.ForumThreads.FindAsync(id);
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