using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StellumbraSite.Server.Models;
using StellumbraSite.Server.Services;
using System.Security.Claims;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AuthController(AppDbContext db)
        {
            _db = db;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserProfile login)
        {
            // You should validate this securely with hashing
            var dbUser = await _db.UserProfiles.FirstOrDefaultAsync(u => u.UserName == login.UserName);
            if (dbUser == null)
            {
                return NotFound("Could not find profile.");

            }

            if (dbUser.Password != login.Password)
            {
                return Unauthorized("Invalid password");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, dbUser.UserName) };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", userPrincipal);
            return Ok();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok();
        }

        // todo
        /*[HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.UserName == userId);
            if (user == null)
            {
                return BadRequest("Invalid user ID");
            }

            var result = await _db.UserProfiles.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed!");
            }

            return BadRequest("Email confirmation failed.");
        }*/
    }
}