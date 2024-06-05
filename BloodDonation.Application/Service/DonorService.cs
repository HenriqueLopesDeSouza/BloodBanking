using BloodBanking.Application.IService;
using BloodBanking.Application.ViewModel;
using BloodBanking.Core.Entities;
using BloodBanking.Core.Repositories;

namespace BloodBanking.Application.Service
{
    public class DonorService : IDonorService
    {
        private IDonorRepository _donorRepository;

        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        public async Task AddDonorAsync(CreateDonorViewModel createDonor)
        {
            Donor donor = MapFromViewModel(createDonor);

            ValidateDonorEligibility(donor);
            await _donorRepository.AddDonorAsync(donor);
        }

        public async Task DeleteDonorAsync(Guid idDonor)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(idDonor);

            if (donor == null)
                throw new ArgumentNullException("Donor not found. ", nameof(donor.Id));

            await _donorRepository.DeleteDonorAsync(donor.Id);
        }

        #region Private 
        private void ValidateDonorEligibility(Donor donor, bool isUpdate = false)
        {
            if (donor == null)
                throw new ArgumentNullException(nameof(donor));

            if (!isUpdate ? _donorRepository.EmailExist(donor) : true)
                ValidateWeight(donor.Weight);

            if (donor.Address == null)
                throw new ArgumentException("Address is required.");

            ValidateAddress(donor.Address);
        }

        public void ValidateDonationEligibility(Donation donation)
        {
            if (donation == null)
                throw new ArgumentNullException(nameof(donation));

            if (donation.Donor == null)
                throw new ArgumentNullException("Donor not found. ", nameof(donation.DonorId));

            ValidateWeight(donation.Donor.Weight);
            ValidateAge(donation.Donor.DateOfBirth);
        }

        private void ValidateAge(DateTime dateOfBirth)
        {
            var age = CalculateAge(dateOfBirth);
            if (age < 18)
                throw new ArgumentException("Minors are not eligible for blood donation.", nameof(dateOfBirth));
        }

        private void ValidateWeight(double weight)
        {
            if (weight < 50)
                throw new ArgumentException("Donor must weigh at least 50KG.", nameof(weight));
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        private void ValidateAddress(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.Street))
                throw new ArgumentException("Street is required.", nameof(address.Street));
            if (string.IsNullOrWhiteSpace(address.City))
                throw new ArgumentException("City is required.", nameof(address.City));
            if (string.IsNullOrWhiteSpace(address.State))
                throw new ArgumentException("State is required.", nameof(address.State));
            if (string.IsNullOrWhiteSpace(address.ZipCode))
                throw new ArgumentException("ZIP Code is required.", nameof(address.ZipCode));
        }

        private Donor MapFromViewModel(CreateDonorViewModel viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var donor = new Donor
            {
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                DateOfBirth = viewModel.DateOfBirth,
                Gender = viewModel.Gender,
                Weight = viewModel.Weight,
                BloodType = viewModel.BloodType,
                RhFactor = viewModel.RhFactor,
            };

            donor.Address = MapAddressFromViewModel(viewModel.Address, donor);

            return donor;
        }

        private Address MapAddressFromViewModel(CreateAddressViewModel addressViewModel, Donor donor)
        {
            if (addressViewModel == null)
            {
                return null;
            }

            return new Address
            {
                Street = addressViewModel.Street,
                City = addressViewModel.City,
                State = addressViewModel.State,
                ZipCode = addressViewModel.ZIPCode,
                Donor = donor
            };
        }

        #endregion
    }
}
