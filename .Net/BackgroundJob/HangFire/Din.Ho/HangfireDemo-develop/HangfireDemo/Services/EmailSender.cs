using HangfireDemo.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace HangfireDemo.Services
{
    public class EmailSender
    {
        private readonly EmailServerSettings _emailServer;
        private readonly DefaultEmailSettings _senderDefaultEmail;

        public EmailSender(IOptionsSnapshot<EmailServerSettings> emailServerSettingsAccessor, 
            IOptionsSnapshot<DefaultEmailSettings> defaultEmailSettingsAccessor)
        {
            _emailServer = emailServerSettingsAccessor.Value;
            _senderDefaultEmail = defaultEmailSettingsAccessor.Get(DefaultEmailSettings.Sender);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var fromAddress = new MailAddress(_senderDefaultEmail.Email, _senderDefaultEmail.Name);
            var toAddress = new MailAddress(email);

            var mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            var client = new SmtpClient(_emailServer.Host, _emailServer.Port);
            client.Credentials = new NetworkCredential(_emailServer.Username, _emailServer.Password);
            client.EnableSsl = true;
            return client.SendMailAsync(mailMessage);
        }
    }
}
