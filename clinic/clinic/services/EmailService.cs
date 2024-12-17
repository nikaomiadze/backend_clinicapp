using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;


namespace clinic.services
{
    public class EmailService
    {
        private readonly string? _sendGridApiKey;
        private readonly string? _fromEmail;
        public EmailService(IConfiguration configuration)
        {
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
            _fromEmail = configuration["SendGrid:FromEmail"];
        }

        public async Task SendVerificationCodeAsync(string toEmail, string verificationCode)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress(_fromEmail, "Your Service");
            var subject = "Your Verification Code";
            var to = new EmailAddress(toEmail);
            var plainTextContent = $"Your verification code is: {verificationCode}";
            var htmlContent = $"<strong>Your verification code is: {verificationCode}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"SendGrid Response: {responseBody}");
            }
            else
            {
                throw new Exception($"Error sending email: Status Code: {response.StatusCode}");
            }
        }


        public class SendGridSettings
        {
            public string? ApiKey { get; set; }
        }
        public class VerificationCodeRequest
        {
            public string? Email { get; set; }
        }
    }
}
