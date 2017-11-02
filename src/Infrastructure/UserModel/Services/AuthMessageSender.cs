using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

namespace Infrastructure.UserModel.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string emailMessage, string adminUserName,
            string adminEmail, string fromPassword, string smtpServer, int smtpServerPort)
        {
            return SendEmailAsync(new List<string> {email}, subject, emailMessage, adminUserName,
                adminEmail, fromPassword, smtpServer, smtpServerPort);
        }
        public Task SendHtmlEmailAsync(string email, string subject, string pathToFile, string adminUserName,
          string adminEmail, string fromPassword, string smtpServer, int smtpServerPort)
        {
            return SendHtmlEmailAsync(new List<string> { email }, subject, pathToFile, adminUserName,
                adminEmail, fromPassword, smtpServer, smtpServerPort);
        }

        public Task SendEmailAsync(List<string> email, string subject, string emailMessage, string adminUserName,
            string adminEmail, string fromPassword, string smtpServer, int smtpServerPort)
        {
            var emailMsg = new MimeMessage();

            emailMsg.From.Add(new MailboxAddress("Hygiena Admin", adminEmail));
            emailMsg.To.Add(new MailboxAddress("", email.Aggregate((a, x) => a + ", " + x)));
            emailMsg.Subject = subject;
            emailMsg.Body = new TextPart("plain") {Text = emailMessage};


            return SendEmail(smtpServer,  smtpServerPort,  emailMsg,  adminEmail,  fromPassword);
        }
        public Task SendHtmlEmailAsync(List<string> email, string subject, string pathToFile, string adminUserName,
            string adminEmail, string fromPassword, string smtpServer, int smtpServerPort)
        {
            var emailMsg = new MimeMessage();

            emailMsg.From.Add(new MailboxAddress("Hygiena Admin", adminEmail));
            emailMsg.To.Add(new MailboxAddress("", email.Aggregate((a, x) => a + ", " + x)));
            emailMsg.Subject = subject;

            var builder = new BodyBuilder {TextBody = @"Report"};

            // Set the plain-text version of the message text

            // generate a Content-Id for the image we'll be referencing
            var contentId = MimeUtils.GenerateMessageId();

            var fileName = Path.GetFileName(pathToFile);
            // Set the html version of the message text
            builder.HtmlBody = $"<p>Report Html<br><center><img src='cid:{contentId}' alt='{fileName}'></center>";

            // Since selfie.jpg is referenced from the html text, we'll need to add it
            // to builder.LinkedResources and then set the Content-Id header value
            builder.LinkedResources.Add(pathToFile);
            builder.LinkedResources[0].ContentId = contentId;

            // We may also want to attach a calendar event for Monica's party...
            builder.Attachments.Add(pathToFile);

            // Now we just need to set the message body and we're done
            emailMsg.Body = builder.ToMessageBody();

            return SendEmail(smtpServer, smtpServerPort, emailMsg, adminEmail, fromPassword);
        }


        public Task SendEmail(string smtpServer, int smtpServerPort, MimeMessage emailMsg, string adminEmail, string fromPassword)
        {

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(smtpServer, smtpServerPort, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                //client.Authenticate(adminEmail, fromPassword);

                client.Send(emailMsg);
                client.Disconnect(true);
            }
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}