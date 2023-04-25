using Authorities.Core.Interfaces;
using Authorities.Infrastructure.DbContext;
using Authorities.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorities.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RolesRepository(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<List<IdentityRole<Guid>>> GetAllAsync(short pageNumber, byte pageSize)
        {
            var query = _roleManager.Roles
                .AsNoTracking();

            return await PagedList<IdentityRole<Guid>>.ToPagedListAsync(query, pageNumber, pageSize, r => r.Id);
        }
    }
}
