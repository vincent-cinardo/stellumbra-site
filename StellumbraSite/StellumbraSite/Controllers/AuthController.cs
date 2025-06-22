using StellumbraSite.Data;
using StellumbraSite.Model;
using StellumbraSite.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string baseCallbackUrl = $"https://localhost:7247/forum/confirm-email";
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AuthController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                return Unauthorized(new
                {
                    errorCode = "InvalidUsername",
                    message = "Username does not exist."
                });
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized(new
                {
                    errorCode = "EmailNotConfirmed",
                    message = "This user has yet to confirm their email."
                });
            }
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }

            if (result.IsLockedOut)
            {
                return Unauthorized(new
                {
                    errorCode = "AccountLocked",
                    message = "Your account is locked."
                });
            }

            if (result.IsNotAllowed)
            {
                return Unauthorized(new
                {
                    errorCode = "LoginNotAllowed",
                    message = "Login is not allowed for this user."
                });
            }

            return Unauthorized(new
            {
                errorCode = "InvalidPassword",
                message = "Incorrect password."
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            string username = register.Username;
            var user = Activator.CreateInstance<ApplicationUser>();
            user.ProfilePicturePath = $"images/profile0.png";

            await _userManager.SetUserNameAsync(user, register.Username);
            await _userManager.SetEmailAsync(user, register.Email);
            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            await EmailHelper.SendEmailConfirmationAsync(user, _userManager, _emailSender, baseCallbackUrl);

            // TODO: Ensure that email is setup. It currently isnt.
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return Ok();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }
        [HttpGet("SendConfirmationLink/{userId}")]
        public async Task<IActionResult> SendConfirmationLink(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            { 
                return NotFound("User was not found.");
            }

            await EmailHelper.SendEmailConfirmationAsync(user, _userManager, _emailSender, baseCallbackUrl);
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