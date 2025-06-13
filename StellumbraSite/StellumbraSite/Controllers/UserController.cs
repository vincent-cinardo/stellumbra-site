using StellumbraSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet("GetUserData/{userID}")]
        public async Task<IActionResult> GetUserData(string userID)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userID);

            if (user == null)
            {
                return NotFound("No user found.");
            }

            return Ok(new UserData { Id = user.Id, Username = user.UserName, ProfilePicturePath = user.ProfilePicturePath});
        }
        [HttpPost("SetProfilePicture")]
        public async Task<IActionResult> SetProfilePicture([FromBody] UserData userData) 
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userData.Id);

            if (user == null)
            {
                return NotFound("No user found.");
            }

            user.ProfilePicturePath = userData.ProfilePicturePath;
            await _userManager.UpdateAsync(user);

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