using Civilians.Core.Misc;

namespace Civilians.Api.ViewModels.Passports
{
    public class PassportViewModel
    {
        public Guid UserId { get; set; } = Guid.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;

        public RegionCodes RegionCode { get; set; }
        public string Number { get; set; } = string.Empty;
    }
}
