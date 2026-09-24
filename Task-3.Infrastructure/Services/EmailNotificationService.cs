using Microsoft.Extensions.Logging;
using Task_3.Application.Interfaces;
using Task_3.Application.Services;

namespace Task_3.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(
            ILogger<EmailNotificationService> logger)
        {
            _logger = logger;
        }

        public async Task NotifyCandidate(int applicationId)
        {
            _logger.LogInformation(
                "Notification sent to candidate for Application Id: {ApplicationId}",
                applicationId);

            await Task.CompletedTask;
        }
    }
}