using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

namespace OttersDatabase.Service
{
    public class MailService
    {
        public void SendVerification(string Email, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No-Reply", "No-Reply@OtterDatabase.com"));
            message.To.Add(new MailboxAddress("", Email));
            //message.Cc.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com"));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = text
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.mailtrap.io", 2525, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("07928879a6b9dd", "be3fbc07a1965b");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
