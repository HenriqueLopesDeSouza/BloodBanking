using BloodBanking.Core.Enums;

namespace BloodBanking.Application.ViewModel
{
    public class CreateDonorViewModel
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public double Weight { get; set; }
        public BloodType BloodType { get; set; }
        public RhFactor RhFactor { get; set; }
        public CreateAddressViewModel Address { get; set; }
    }
}
