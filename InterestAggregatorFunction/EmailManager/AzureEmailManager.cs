using Azure;
using Azure.Communication.Email;
using InterestAggregatorFunction.Services.EmailManager;
using System;

namespace InterestAggregatorFunction.Services.AzureEmailManager
{
    public class AzureEmailManager : IEmailManager
    {
        private const string fromEmail = "DoNotReply@41cfda87-da39-4c60-9653-399c359808cc.azurecomm.net";
        private const string toEmail = "alexander_hoare@hotmail.co.uk";
        private readonly string subject = "Your Evening Feed " + DateTime.Now.ToShortDateString();
        private readonly string connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AzureEmailApiKey");

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
