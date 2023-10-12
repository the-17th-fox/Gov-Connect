namespace Civilians.Api.ViewModels.Users
{
    public class ShortUserViewModel
    {
        public string Email { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
    }
}
