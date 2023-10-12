namespace Authorities.Api.ViewModels.Users
{
    public class ShortUserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
