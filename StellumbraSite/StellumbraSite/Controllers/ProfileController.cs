using StellumbraSite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StellumbraSite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProfileController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _db.Users.Select(x => new ApplicationUser
            {
                Id = x.Id,
                UserName = x.UserName,
            }).ToListAsync();
            return Ok(result);
        }
        [HttpGet("GetUserByName/{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var user = await _db.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}