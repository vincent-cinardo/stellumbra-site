using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Server.Models;
using Microsoft.EntityFrameworkCore;
using StellumbraSite.Server.Services;

namespace StellumbraSite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController: ControllerBase
    {
        private readonly AppDbContext _db;

        public ProfileController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var result = await _db.UserProfiles.Select(x => new UserProfile
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password
            }).ToListAsync();
            return Ok(result);
        }
        [HttpGet("GetUserByName/{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var user = await _db.UserProfiles
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
