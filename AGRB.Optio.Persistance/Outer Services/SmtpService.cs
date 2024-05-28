using System.Net;
using System.Net.Mail;

namespace RGBA.Optio.Domain.Services.Outer_Services
{
    public  class SmtpService
    {
        public void  SendMessage(string to,string  subject,string text)
        {
            const string senderEmail = "rgbasolution@gmail.com";
            const string senderPassword = "txbq xncu twoj parr";

            var recipientEmail = to;

            const string smtpAddress = "smtp.gmail.com";
            var portNumber = 587;


            var smtpClient = new SmtpClient(smtpAddress, portNumber);
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage(senderEmail, recipientEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = text;
            mailMessage.IsBodyHtml = true;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send: {ex.Message}");
            }
            finally
            {
                smtpClient.Dispose();
                mailMessage.Dispose();
            }
        }
    }
}
