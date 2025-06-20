using StellumbraSite.Data;
using StellumbraSite.Model;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("GetPostCount/{threadID}")]
        public async Task<IActionResult> GetThreadCount(int threadID)
        {
            int count = await _db.ForumPosts
                .Where(x => x.ThreadID == threadID)
                .CountAsync();
            return Ok(count);
        }
        [HttpGet("GetUserReplyCount/{posterID}")]
        public async Task<IActionResult> GetUserReplyCount(string posterID)
        {
            int count = await _db.ForumPosts
                .CountAsync(x => x.PosterId == posterID && !x.IsFirstPost);
            return Ok(count);
        }
        [HttpGet("GetPosts/{threadID}/{page}/{pageSize}")]
        public async Task<IActionResult> GetPosts(int threadID, int page, int pageSize)
        {
            try
            {
                var result = await _db.ForumPosts
                .Where(x => x.ThreadID == threadID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ForumPost
                {
                    Id = x.Id,
                    ThreadID = x.ThreadID,
                    PosterId = x.PosterId,
                    Content = x.Content,
                    IsFirstPost = x.IsFirstPost,
                    DateTime = x.DateTime,
                    ForumThread = x.ForumThread,
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
        [HttpGet("GetFirstPost/{threadID}")]
        public async Task<IActionResult> GetFirstPost(int threadID)
        {
            try
            {
                var result = await _db.ForumPosts.SingleOrDefaultAsync(x => x.ThreadID == threadID && x.IsFirstPost == true);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpPost("SubmitPost")]
        public async Task<IActionResult> SubmitPost([FromBody] ForumPostDto forumPostDto)
        {
            ForumPost forumPost = new ForumPost();
            forumPost.Id = forumPostDto.Id;
            forumPost.ThreadID = forumPostDto.ThreadID;
            forumPost.PosterId = forumPostDto.PosterId;
            forumPost.Content = forumPostDto.Content;
            forumPost.IsFirstPost = forumPostDto.IsFirstPost;

            await _db.ForumPosts.AddAsync(forumPost);
            await _db.SaveChangesAsync();
            return Ok(forumPost);
        }
        [HttpGet("DeletePost/{Id}")]
        public async Task<IActionResult> DeletePost(int id)
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