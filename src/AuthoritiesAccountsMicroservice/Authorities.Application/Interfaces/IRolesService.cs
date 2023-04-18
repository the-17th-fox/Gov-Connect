using Authorities.Application.ViewModels.Pagination;
using Microsoft.AspNetCore.Identity;

namespace Authorities.Application.Interfaces
{
    public interface IRolesService
    {
        public Task<IdentityRole<Guid>> GetByIdAsync(Guid id);
        public Task<IdentityRole<Guid>> GetByNameAsync(string roleName);
        public Task<List<IdentityRole<Guid>>> GetAllAsync(PaginationParametersViewModel paginationParameters);
        public Task CreateAsync(string roleName);
        public Task DeleteAsync(string roleName);
        public Task UpdateAsync(string oldRoleName, string newRoleName);

    }
}
