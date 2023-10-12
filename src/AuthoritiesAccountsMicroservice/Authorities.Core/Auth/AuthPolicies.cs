namespace Authorities.Core.Auth
{
    public class AuthPolicies
    {
        public static List<string> DefaultRights => new() 
        {
            AuthRoles.MIA,
            AuthRoles.MD,
            AuthRoles.ME,
            AuthRoles.MES,
            AuthRoles.MH,
            AuthRoles.MR
        };


        public static List<string> Administrators => new()
        {
            AuthRoles.Administrator
        };
    }
}
