using BloodBanking.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodBanking.Core.Entities
{
    public class BloodStock
    {
        [Key]
        public Guid Id { get; set; }
        public BloodType BloodType { get; set; }
        public RhFactor RhFactor { get; set; }
        public int QuantityML { get; set; }
    }
}
