using BloodBanking.Core.Entities;
using BloodBanking.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BloodBanking.Infrastructure.Persistence.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly BloodDonationDbContext _context;

        public DonorRepository(BloodDonationDbContext dbContext)
        {
            _context = dbContext;
        }

        #region Public
        public bool EmailExist(Donor donor)
        {
            if (_context.Donors.Any(d => d.Email == donor.Email))
            {
                throw new ArgumentException("A donor with the same email already exists.", nameof(donor.Email));
            }

            return true;
        }

        public async Task AddDonorAsync(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }



        public async Task<Donor> GetDonorByIdAsync(Guid donorId)
        {
            if (donorId == Guid.Empty)
                throw new ArgumentException("Invalid donor ID.", nameof(donorId));

            return await _context.Donors.Include(d => d.Address).FirstOrDefaultAsync(d => d.Id == donorId);
        }

        public async Task<IEnumerable<Donor>> GetAllDonorsAsync()
        {
            return await _context.Donors
                            .Include(d => d.Address)
                            .ToListAsync(); 
        }

        public async Task DeleteDonorAsync(Guid donorId)
        {
            var donor = await _context.Donors.FindAsync(donorId);
            if (donor == null)
                throw new ArgumentException("Error delete the ", nameof(donorId));

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
