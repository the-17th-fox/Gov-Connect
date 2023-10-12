using System.ComponentModel.DataAnnotations;

namespace Civilians.Application.ViewModels.Civilians
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
