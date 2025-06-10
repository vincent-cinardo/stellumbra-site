using StellumbraSite.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StellumbraSite.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text;

namespace StellumbraSite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result);
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

            await _userManager.SetUserNameAsync(user, register.Username);
            await _userManager.SetEmailAsync(user, register.Email);
            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            /*var callbackUrl = Navigation.GetUriWithQueryParameters(
                Navigation.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });*/

            // TODO: Implement emailing later on.
            //await EmailSender.SendConfirmationLinkAsync(user, Registration.Email, HtmlEncoder.Default.Encode(callbackUrl));

            // TODO: Ensure that email is setup. It currently isnt.
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                // TODO: Uncomment later
                /*RedirectManager.RedirectTo(
                    "Account/RegisterConfirmation",
                    new() { ["email"] = Registration.Email, ["returnUrl"] = ReturnUrl });*/
                return Ok();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
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