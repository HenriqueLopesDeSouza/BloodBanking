
namespace BloodBanking.Application.ViewModel
{
    public class DonationReportViewModel
    {
        public DateTime DonationDate { get; set; }
        public int QuantityML { get; set; }
        public required string DonorFullName { get; set; }
        public required string DonorEmail { get; set; }
        public required string DonorBloodType { get; set; }
        public required string DonorRhFactor { get; set; }
    }
}
