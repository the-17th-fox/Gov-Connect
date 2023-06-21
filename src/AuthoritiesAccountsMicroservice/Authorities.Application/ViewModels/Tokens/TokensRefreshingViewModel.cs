using System.ComponentModel.DataAnnotations;

namespace Authorities.Application.ViewModels.Tokens
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class TokensRefreshingViewModel
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        public Guid RefreshToken { get; set; } = Guid.Empty;
    }
}
