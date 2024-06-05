using BloodBanking.Application.ViewModel;

namespace BloodBanking.Application.IService
{
    public interface IDonationService
    {
        Task AddDonationAsync(CreateDonationViewModel donationViewModel);

    }
}
