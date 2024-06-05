using System.ComponentModel.DataAnnotations;

namespace BloodBanking.Core.Enums
{
    public enum RhFactor
    {
        [Display(Name = "Positive")]
        Positive,
        [Display(Name = "Negative")]
        Negative
    }
}
