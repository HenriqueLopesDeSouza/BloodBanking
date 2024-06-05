using BloodBanking.Application.ViewModel;

namespace BloodBanking.Application.IService
{
    public interface IDonorService
    {
        Task AddDonorAsync(CreateDonorViewModel createDonor);
        Task DeleteDonorAsync(Guid idDonor);

    }
}
