using System.ComponentModel.DataAnnotations;

namespace BloodBanking.Core.Enums
{
    public enum BloodType
    {
        [Display(Name = "A")]
        A,
        [Display(Name = "B")]
        B,
        [Display(Name = "AB")]
        AB,
        [Display(Name = "O")]
        O
    }
}
