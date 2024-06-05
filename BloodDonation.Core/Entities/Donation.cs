using System.ComponentModel.DataAnnotations;

namespace BloodBanking.Core.Entities
{
    public class Donation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DonorId { get; set; } 
        public DateTime DonationDate { get; set; }
        public int QuantityML { get; set; }
        public required Donor Donor { get; set; }
    }
}
