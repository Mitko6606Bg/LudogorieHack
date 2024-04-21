using MimeKit;

namespace BusinessContactsPlatform.Controllers
{
    public class Email
    {
        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("B-Contact", "businesscontact905@gmail.com"));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("businesscontact905@gmail.com", "nruu aotr wcts iyzu");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
