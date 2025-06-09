using StellumbraSite.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            // You should validate this securely with hashing
            var dbUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == login.Username);
            if (dbUser == null)
            {
                return NotFound("Could not find profile.");
            }

            var passwordIsCorrect = await _userManager.CheckPasswordAsync(dbUser, login.Password);

            if (!passwordIsCorrect)
            {
                return Unauthorized("Invalid password");
            }

            var roles = await _userManager.GetRolesAsync(dbUser);
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, dbUser.UserName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(30),
                IsPersistent = true,
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var cookies = Request.Cookies;
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
            return Ok();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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