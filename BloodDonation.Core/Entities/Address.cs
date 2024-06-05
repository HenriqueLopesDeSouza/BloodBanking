using System.ComponentModel.DataAnnotations;

namespace BloodBanking.Core.Entities
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public Donor Donor { get; set; }
    }
}
