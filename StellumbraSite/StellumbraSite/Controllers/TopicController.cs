using StellumbraSite.Data;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TopicController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet("GetTopic/{topicName}")]
        public async Task<IActionResult> GetTopic(string topicName)
        {
            try
            {
                var result = await _db.ForumTopics
                .Where(x => x.TopicName == topicName)
                .Select(x => new ForumTopic
                {
                    TopicName = x.TopicName,
                    TopicShownName = x.TopicShownName,
                    Description = x.Description
                })
                .ToListAsync();
                return Ok(result[0]);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpGet("GetAllTopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            try
            {
                var result = await _db.ForumTopics
                .Select(x => new ForumTopic
                {
                    TopicName = x.TopicName,
                    TopicShownName = x.TopicShownName,
                    Description = x.Description
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