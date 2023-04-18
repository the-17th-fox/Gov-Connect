using Authorities.Application.Interfaces;
using Authorities.Application.ViewModels.Pagination;
using Authorities.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Authorities.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RolesService(RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager ?? throw  new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IdentityRole<Guid>> GetByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
                throw new KeyNotFoundException("Role with the specified id does not exist.");
            }

            return role;
        }

        public async Task<IdentityRole<Guid>> GetByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                throw new KeyNotFoundException("Role with the specified name does not exist.");
            }

            return role;
        }

        public async Task<List<IdentityRole<Guid>>> GetAllAsync(PaginationParametersViewModel paginationParameters)
        {
            return await _unitOfWork.RolesRepository.GetAllAsync(
                paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }

        public async Task CreateAsync(string roleName)
        {
            var role = new IdentityRole<Guid>(roleName);
            
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("New role creation has failed.");
;           }
        }

        public async Task DeleteAsync(string roleName)
        {
            var role = await GetByNameAsync(roleName);

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("Role deletion has failed.");
            }
        }

        public async Task UpdateAsync(string oldRoleName, string newRoleName)
        {
            var existingRole = await GetByNameAsync(oldRoleName);

            existingRole.Name = newRoleName;
            existingRole.NormalizedName = newRoleName.Normalize();

            await _roleManager.UpdateAsync(existingRole);
        }
    }
}
