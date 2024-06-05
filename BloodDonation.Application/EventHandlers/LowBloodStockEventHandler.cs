using BloodBanking.Application.IService;
using BloodBanking.Core.DomainEvents;
using MediatR;

namespace BloodBanking.Application.EventHandlers
{
    public class LowBloodStockEventHandler : INotificationHandler<LowBloodStockEvent>
    {
        private readonly IEmailService _emailService;

        public LowBloodStockEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(LowBloodStockEvent notification, CancellationToken cancellationToken)
        {
            var subject = "Low Blood Stock Warning";
            var message = $"The blood stock for BloodType {notification.BloodType} and RhFactor {notification.RhFactor} is below the minimum threshold.";

            await _emailService.SendEmailAsync("henriqueldes@gmail.com", subject, message);
        }
    }
}
