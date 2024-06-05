
namespace BloodBanking.Application.ViewModel
{
    public class CreateDonationViewModel
    {
        public Guid DonorId { get; set; }
        public DateTime DonationDate { get; set; }
        public int QuantityML { get; set; }
    }
}
