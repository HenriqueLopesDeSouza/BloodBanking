using BloodBanking.Core.Entities;

namespace BloodBanking.Core.Repositories
{
    public interface IDonationRepository
    {
        Task AddAsync(Donation donation);
        Task<Donation> GetDonorByIdAsync(Guid donationId);
        Task<IEnumerable<Donation>> GetAllDonorsAsync();
        Task<Donation> GetLastDonationAsync(Guid donorId);
        Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(Guid donorId);
        Task<IEnumerable<Donation>> GetDonationsAfterDateAsync(DateTime dateFrom);
    }
}
