namespace Civilians.Core.Auth
{
    public class AuthPolicies
    {
        public static List<string> DefaultRights => new()
        {
            AuthRoles.DefaultUser
        };

        public static List<string> Administrators => new()
        {
            AuthRoles.Administrator
        };
    }
}
