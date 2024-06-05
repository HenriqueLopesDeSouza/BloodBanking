using BloodBanking.Core.Entities;

namespace BloodBanking.Core.Repositories
{
    public interface IDonorRepository
    {
        bool EmailExist(Donor donor);
        Task AddDonorAsync(Donor donor);
        Task DeleteDonorAsync(Guid donorId);
        Task<Donor> GetDonorByIdAsync(Guid donorId);
        Task<IEnumerable<Donor>> GetAllDonorsAsync();
    }
}
