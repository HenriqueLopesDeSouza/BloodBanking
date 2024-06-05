using BloodBanking.Core.DomainEvents;
using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BloodBanking.Infrastructure.Persistence.Repositories
{
    public class BloodStockRepository : IBloodStockRepository
    {
        private readonly BloodDonationDbContext _context;
        private const int MinimumStockQuantity = 100;
        private readonly IMediator _mediator;
        public BloodStockRepository(BloodDonationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BloodStock> GetByIdAsync(Guid id)
        {
            return await _context.BloodStocks.FindAsync(id);
        }

        public async Task<IEnumerable<BloodStock>> GetAllAsync()
        {
            return await _context.BloodStocks.ToListAsync();
        }
        public async Task<BloodStock> GetByBloodTypeAndRhFactorAsync(BloodType bloodType, RhFactor rhFactor)
        {
            return await _context.BloodStocks
                .FirstOrDefaultAsync(bs => bs.BloodType == bloodType && bs.RhFactor == rhFactor);
        }

        public async Task DecreaseQuantityAsync(BloodType bloodType, RhFactor rhFactor, int quantityML)
        {
            var bloodStock = await GetByBloodTypeAndRhFactorAsync(bloodType, rhFactor);
            if (bloodStock == null)
            {
                throw new ArgumentException($"No blood stock found for BloodType {bloodType} and RhFactor {rhFactor}.");
            }

            if (quantityML == 0)
            {
                throw new ArgumentException("Quantity ML can't be 0.");
            }

            if (bloodStock.QuantityML < quantityML)
            {
                throw new ArgumentException("Insufficient blood stock to fulfill the request.");
            }

            bloodStock.QuantityML -= quantityML;
            await _context.SaveChangesAsync();

            if (bloodStock.QuantityML <= MinimumStockQuantity)
            {
                var lowStockEvent = new LowBloodStockEvent(bloodType, rhFactor, bloodStock.QuantityML);
                await _mediator.Publish(lowStockEvent);
            }
        }
    }
}
