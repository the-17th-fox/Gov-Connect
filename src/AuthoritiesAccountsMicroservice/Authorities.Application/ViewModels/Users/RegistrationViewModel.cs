using System.ComponentModel.DataAnnotations;

namespace Authorities.Application.ViewModels.Users
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class RegistrationViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
