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
        [HttpGet("GetThreadCount/{postID}")]
        public async Task<IActionResult> GetPostCount(int postID)
        {
            int count = await _db.ForumThreads
                .Where(x => x.PostID == postID)
                .CountAsync();
            return Ok(count);
        }
        [HttpGet("GetThreads/{postID}/{page}/{pageSize}")]
        public async Task<IActionResult> GetThreads(int postID, int page, int pageSize)
        {
            try
            {
                var result = await _db.ForumThreads
                .Where(x => x.PostID == postID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ForumThread
                {
                    Id = x.Id,
                    PostID = x.PostID,
                    PosterID = x.PosterID,
                    Content = x.Content,
                    IsFirstThread = x.IsFirstThread,
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
        [HttpGet("GetFirstThread/{postID}")]
        public async Task<IActionResult> GetFirstThread(int postID)
        {
            try
            {
                var result = await _db.ForumThreads.SingleOrDefaultAsync(x => x.PostID == postID && x.IsFirstThread == true);
                return Ok(result);
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
            return StatusCode(500, $"Internal server error: The thread whose ID is {id} did not exist.");
        }
    }
}