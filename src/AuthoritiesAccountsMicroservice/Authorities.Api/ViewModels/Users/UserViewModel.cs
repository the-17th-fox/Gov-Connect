namespace Authorities.Api.ViewModels.Users
{
    public class UserViewModel : ShortUserViewModel
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = null!;
    }
}
