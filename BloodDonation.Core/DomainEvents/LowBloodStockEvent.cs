using BloodBanking.Core.Enums;
using MediatR;

namespace BloodBanking.Core.DomainEvents
{
    public class LowBloodStockEvent : INotification
    {
        public BloodType BloodType { get; }
        public RhFactor RhFactor { get; }
        public int QuantityML { get; }

        public LowBloodStockEvent(BloodType bloodType, RhFactor rhFactor, int quantityML)
        {
            BloodType = bloodType;
            RhFactor = rhFactor;
            QuantityML = quantityML;
        }
    }
}
