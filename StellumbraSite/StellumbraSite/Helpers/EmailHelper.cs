using System.Text;
using StellumbraSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace StellumbraSite.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendEmailConfirmationAsync(ApplicationUser user, UserManager<ApplicationUser> userManager, IEmailSender emailSender, string baseCallbackUrl)
        {
            var userId = await userManager.GetUserIdAsync(user);
            var email = await userManager.GetEmailAsync(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Use config or base URL abstraction later
            var callbackUrl = $"{baseCallbackUrl}?userId={userId}&code={code}";

            await emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
        }
    }
}
