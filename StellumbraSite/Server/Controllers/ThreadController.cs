﻿using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Server.Models;
using Microsoft.EntityFrameworkCore;
using StellumbraSite.Server.Services;

namespace StellumbraSite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ThreadController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("GetThreadCount/{postID}")]
        public async Task<IActionResult> GetPostCount(int postID)
        {
            int count = await _db.ForumThreads.CountAsync();
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