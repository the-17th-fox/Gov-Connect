using System.ComponentModel.DataAnnotations;

namespace Authorities.Application.ViewModels.Users
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
