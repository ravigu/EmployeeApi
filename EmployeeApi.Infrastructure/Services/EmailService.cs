using EmployeeApi.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
            private readonly EmailSettings _emailSettings;

            public EmailService(
                IOptions<EmailSettings> emailSettings)
            {
                _emailSettings = emailSettings.Value;
            }

            public async Task SendWelcomeEmail(string email)
            {
                var message = new MimeMessage();

                message.From.Add(  MailboxAddress.Parse(_emailSettings.From));

                message.To.Add(MailboxAddress.Parse(email));

                message.Subject = "Welcome";

                message.Body = new TextPart("plain")
                {
                    Text = "Welcome to Employee API."
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.Port,
                    MailKit.Security.SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(
                    _emailSettings.Username,
                    _emailSettings.Password);

                await smtp.SendAsync(message);

                await smtp.DisconnectAsync(true);
            }
        }
    }
