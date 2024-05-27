using System.Net;
using System.Net.Mail;

namespace RGBA.Optio.Domain.Services.Outer_Services
{
    public  class SmtpService
    {
        public void  SendMessage(string to,string  subject,string text)
        {
            string senderEmail = "rgbasolution@gmail.com";
            string senderPassword = "txbq xncu twoj parr";

            string recipientEmail = to;

            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;


            SmtpClient smtpClient = new SmtpClient(smtpAddress, portNumber);
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail);
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
