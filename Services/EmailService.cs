using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using SystemBackend.Models.Entities;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly Configurations.EmailSettings _emailSettings;

        public EmailService(IOptions<Configurations.EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true; // Set to true if your body contains HTML

                    using (SmtpClient smtp = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort))
                    {
                        smtp.EnableSsl = _emailSettings.EnableSsl;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SmtpPassword);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Send(mail);
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine(smtpEx.Message);
                throw new Exception($"SMTP Error sending email: {smtpEx.Message}", smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error sending email.", ex);
            }
        }

        public void SendReceiveCreateAccountRequest(PendingUser pendingUser)
        {
            string body = $"We have received your request to create an account with username \"{pendingUser.Username}\".\n"
                + "Your account will be verified soon. Once this is done, we will send you a notification email.";

            SendEmail(pendingUser.Email, "Account Request Received", body);
        }

        public void SendApproveCreateAccountRequest(PendingUser pendingUser)
        {
            string body = $"Your account of username \"{pendingUser.Username}\" has been approved.\n"
                + "You are now able to sign in our system!";

            SendEmail(pendingUser.Email, "Account Request Approved", body);
        }

        public void SendDenyCreateAccountRequest(PendingUser pendingUser)
        {
            string body = $"Your request for creating account of username \"{pendingUser.Username}\" has been denied.\n";

            SendEmail(pendingUser.Email, "Account Request Approved", body);
        }

        
    }
}
