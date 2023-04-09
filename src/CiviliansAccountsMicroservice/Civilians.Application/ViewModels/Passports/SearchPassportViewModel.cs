using System.ComponentModel.DataAnnotations;

namespace Civilians.Application.ViewModels.Passports
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class SearchPassportViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Patronymic { get; set; } = string.Empty;
    }
}
