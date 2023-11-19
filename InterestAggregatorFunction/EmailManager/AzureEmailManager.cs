using Azure;
using Azure.Communication.Email;

namespace InterestAggregatorFunction.Services
{
    public class AzureEmailManager : IEmailManager
    {
        private const string fromEmail = "DoNotReply@b19343e0-1d0d-40de-8ae4-b6e2c21672de.azurecomm.net";
        private const string toEmail = "alexander_hoare@hotmail.co.uk";
        private readonly string subject = "Your Evening Feed " + DateTime.Now.ToShortDateString();
        private readonly string connectionString;

        public AzureEmailManager()
        {
            connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AzureEmailApiKey") ?? throw new KeyNotFoundException("Cannot load Azure Email Api key");
        }
        public void SendEmail(string body)
        {
            Console.WriteLine("Preparing email");
            var emailClient = new EmailClient(connectionString);
            emailClient.Send(
                WaitUntil.Completed,
                fromEmail,
                toEmail,
                subject,
                body);
            Console.WriteLine("Email sent!");
        }
    }
}
