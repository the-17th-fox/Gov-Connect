using Civilians.Core.Misc;
using System.ComponentModel.DataAnnotations;

namespace Civilians.Core.ViewModels.Passports
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class UpdatePassportViewModel
    {
        [Required]
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Patronymic { get; set; } = string.Empty;

        [Required]
        public RegionCodes RegionCode { get; set; }
        [Required]
        public string Number { get; set; } = string.Empty;
    }
}
