using Microsoft.AspNetCore.Identity;

namespace Authorities.Core.Auth
{
    public class AuthRoles
    {
        public const string MES = "MinOfEmergencySituations";
        public const string MIA = "MinOfInternalAffairs";
        public const string MH = "MinOfHealth";
        public const string MD = "MinOfDefence";
        public const string ME = "MinOfEducation";
        public const string MR = "MunicipalRepresentative";

        public const string Administrator = "Administrator";

        public static List<IdentityRole<Guid>> Roles { get; } = new()
        {
            new IdentityRole<Guid>(MES) { Id = Guid.NewGuid(), NormalizedName = MES.Normalize() },
            new IdentityRole<Guid>(MIA) { Id = Guid.NewGuid(), NormalizedName = MIA.Normalize() },
            new IdentityRole<Guid>(MH) { Id = Guid.NewGuid(), NormalizedName = MH.Normalize() },
            new IdentityRole<Guid>(MD) { Id = Guid.NewGuid(), NormalizedName = MD.Normalize() },
            new IdentityRole<Guid>(ME) { Id = Guid.NewGuid(), NormalizedName = ME.Normalize() },
            new IdentityRole<Guid>(MR) { Id = Guid.NewGuid(), NormalizedName = MR.Normalize() },

            new IdentityRole<Guid>(Administrator) { Id = Guid.NewGuid(), NormalizedName = Administrator.Normalize() }
        };
    }
}
