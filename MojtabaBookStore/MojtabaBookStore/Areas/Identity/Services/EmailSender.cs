using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential()
                {
                    UserName = "m.mindhorizon",
                    Password = "moji@1987"
                };
                client.Credentials = credential;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress("m.mindhorizon@gmail.com");
                    emailMessage.Subject = subject;
                    emailMessage.IsBodyHtml = true;
                    emailMessage.Body = message;
                    client.Send(emailMessage);
    
                }
                await Task.CompletedTask;
            }
        }
    }
}
