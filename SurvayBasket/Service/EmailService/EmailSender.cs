

namespace SurvayBasket.Service.EmailService
{
    public class EmailSender(IOptions<MailSettings> MailSetting) : IEmailSender
    {
        private readonly MailSettings mailSetting = MailSetting.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSetting.Mail),
                Subject = subject
            };

            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage

            };
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailSetting.Host, mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSetting.Mail, mailSetting.Password);
            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);

        }
    }
}
