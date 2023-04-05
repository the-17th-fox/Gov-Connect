using Civilians.Core.Misc;
using System.ComponentModel.DataAnnotations;

namespace Civilians.Core.ViewModels.Civilians
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class RegistrationViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Patronymic { get; set; } = string.Empty;

        [Required]
        public RegionCodes PassportRegionCode { get; set; }

        [Required]
        public string PassportNumber { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
