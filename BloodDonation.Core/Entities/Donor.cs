using BloodBanking.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodBanking.Core.Entities
{
    public class Donor
    {
        [Key]
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public double Weight { get; set; }
        public BloodType BloodType { get; set; }
        public RhFactor RhFactor { get; set; }
        public List<Donation>? Donations { get; set; }
        public Address Address { get; set; }
        public Guid AddressId { get; set; }

      
    }
}
