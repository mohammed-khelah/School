using System.Net.Mail;
using System.Net;

namespace TestAppSchool.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        public async Task Send(string email, string subject, string htmlMessage)
        {
            var fMail = "betaaspcourse@gmail.com";
            var fPassword = "Ahmed11$";

            var theMsg = new MailMessage();
            theMsg.From = new MailAddress(fMail);
            theMsg.Subject = subject;
            theMsg.To.Add(email);
            theMsg.Body = $"<html><body>{htmlMessage}</body></html>";
            theMsg.IsBodyHtml = true;

            var smtpClint = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fMail, fPassword),
                Port = 587
            };

            smtpClint.Send(theMsg);
        }
    }
}
