using System.ComponentModel.DataAnnotations;
namespace BloodBanking.Core.Enums
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female,
        [Display(Name = "Other")]
        Other
    }
}
