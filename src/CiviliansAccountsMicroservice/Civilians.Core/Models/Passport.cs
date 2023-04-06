using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Civilians.Core.Models
{
    [Index(nameof(FirstName), nameof(LastName), nameof(Patronymic))]
    public class Passport
    {
        public Guid Id { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Patronymic { get; set; } = string.Empty;

        // Passport number
        [Required]
        public string Region { get; set; } = string.Empty;
        [Required]
        public string Number { get; set; } = string.Empty;
    }
}
