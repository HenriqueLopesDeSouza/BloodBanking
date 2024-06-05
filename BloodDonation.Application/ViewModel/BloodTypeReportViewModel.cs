using BloodBanking.Core.Enums;

namespace BloodBanking.Application.ViewModel
{
    public class BloodTypeReportViewModel
    {
        public BloodType BloodType { get; set; }
        public RhFactor RhFactor { get; set; }
        public int TotalQuantity { get; set; }
    }
}
