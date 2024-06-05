using BloodBanking.Core.Entities;
using BloodBanking.Core.Repositories;
using BloodBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodBanking.Infrastructure.Persistence.Repositories
{
    public class DonationRepository : IDonationRepository
    {
        private readonly BloodDonationDbContext _context;

        public DonationRepository(BloodDonationDbContext dbContext)
        {
            _context = dbContext;
        }

        #region Public
        public async Task AddAsync(Donation donation)
        {
          
            await _context.Donations.AddAsync(donation);
            await _context.SaveChangesAsync();

            var bloodStock = await _context.BloodStocks
           .FirstOrDefaultAsync(bs => bs.BloodType == donation.Donor.BloodType && bs.RhFactor == donation.Donor.RhFactor);

            if (bloodStock != null)
            {
                // Update existing blood stock
                bloodStock.QuantityML += donation.QuantityML;
                _context.BloodStocks.Update(bloodStock);
            }
            else
            {
                // Create a new blood stock entry
                bloodStock = new BloodStock
                {
                    Id = Guid.NewGuid(),
                    BloodType = donation.Donor.BloodType,
                    RhFactor = donation.Donor.RhFactor,
                    QuantityML = donation.QuantityML
                };
                await _context.BloodStocks.AddAsync(bloodStock);
            }

            await _context.SaveChangesAsync();

        }

        public async Task<Donation> GetDonorByIdAsync(Guid donationId)
        {
            if (donationId == Guid.Empty)
                throw new ArgumentException("Invalid Donation ID.", nameof(donationId));

            return await _context.Donations.FirstOrDefaultAsync(d => d.Id == donationId);
        }

        public async Task<IEnumerable<Donation>> GetAllDonorsAsync()
        {
            return await _context.Donations.ToListAsync();
        }

        public async Task<Donation> GetLastDonationAsync(Guid donorId)
        {
            return await _context.Donations
                .Where(d => d.DonorId == donorId)
                .OrderByDescending(d => d.DonationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(Guid donorId)
        {
            return await _context.Donations
                .Where(d => d.DonorId == donorId)
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Donation>> GetDonationsAfterDateAsync(DateTime dateFrom)
        {
            return await _context.Donations
                .Where(d => d.DonationDate >= dateFrom)
                .Include(d => d.Donor)
                .ToListAsync();
        }

        #endregion
    }
}
