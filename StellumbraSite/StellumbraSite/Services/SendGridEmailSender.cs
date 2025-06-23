using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

public class SendGridEmailSender : IEmailSender
{
    public SendGridEmailSender() { }

    // TODO: This works but ends up in spam. Need to use an actual domain/upgrade plan in sendgrid.
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // TODO: To set up api key again in cmd, set SENDGRID_API_KEY=your_key_here
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.User);
        /* var options = new SendGridClientOptions
        {
            ApiKey = apiKey
        };
        options.SetDataResidency("eu");
        var client = new SendGridClient(options); */
    // uncomment the above 6 lines if you are sending mail using a regional EU subuser
    // and remove the client declaration just below

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("cintechstudios@gmail.com", "Stellumbra Team");
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
        return client.SendEmailAsync(msg);
    }
}
