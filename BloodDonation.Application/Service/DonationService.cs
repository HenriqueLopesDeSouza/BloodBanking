using BloodBanking.Application.IService;
using BloodBanking.Application.ViewModel;
using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Core.Repositories;

namespace BloodBanking.Application.Service
{
    public class DonationService : IDonationService
    {
        private IDonationRepository _donationRepository;
        private IDonorRepository _donorRepository;

        public DonationService(IDonationRepository donationRepository, IDonorRepository donorRepository)
        {
            _donationRepository = donationRepository;
            _donorRepository = donorRepository;
        }

        public async Task AddDonationAsync(CreateDonationViewModel donationViewModel)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(donationViewModel.DonorId);
            if (donor == null)
            {
                throw new ArgumentException($"No donor found with ID {donationViewModel.DonorId}.");
            }

            CalculateAge(donor.DateOfBirth);
     
            var donation = MapAddressFromViewModel(donationViewModel, donor);
            var lastDonation = await _donationRepository.GetLastDonationAsync(donation.DonorId);

            IsDonationIntervalRespected(donation, lastDonation);
            ValidateDonationQuantityRange(donation.QuantityML);

            await _donationRepository.AddAsync(donation);
        }

        private void IsDonationIntervalRespected(Donation newDonation, Donation lastDonation)
        {
            var interval = GetNextDonationInterval(newDonation.Donor.Gender);
            if (lastDonation != null) 
            {
                var daysSinceLastDonation = (newDonation.DonationDate - lastDonation.DonationDate).TotalDays;
                if (daysSinceLastDonation < interval)
                {
                    throw new ArgumentException(
                        $"Donation interval not respected for {newDonation.Donor.Gender}. " +
                        $"The next donation can only occur after {interval} days."
                    );
                }
            }                 
        }

        public void ValidateDonationQuantityRange(int quantityML)
        {
            if (quantityML < 420 || quantityML > 470)
            {
                throw new ArgumentException("The quantity of blood donated must be between 420ml and 470ml.", nameof(quantityML));
            }
        }

        private int GetNextDonationInterval(Gender gender)
        {
            return gender == Gender.Female? 90 : 60;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                throw new ArgumentException($"The donor is not old enough. Minimum age required: 18 years.");
            }

            return age;
        }

        private Donation MapAddressFromViewModel(CreateDonationViewModel createDonationViewModel, Donor donor)
        {
            if (createDonationViewModel == null)
            {
                return null;
            }

            return new Donation
            {
                DonorId = createDonationViewModel.DonorId,
                DonationDate = createDonationViewModel.DonationDate,
                QuantityML = createDonationViewModel.QuantityML,
                Donor = donor
            };
        }
    }
}
