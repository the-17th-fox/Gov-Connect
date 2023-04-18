using Microsoft.AspNetCore.Identity;

namespace Authorities.Core.Interfaces
{
    public interface IRolesRepository
    {
        public Task<List<IdentityRole<Guid>>> GetAllAsync(short pageNumber, byte pageSize);
    }
}