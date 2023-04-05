using Civilians.Core.Models;
using Civilians.Core.ViewModels.Civilians;
using Civilians.Core.ViewModels.Tokens;

namespace Civilians.Application.Interfaces
{
    public interface ICiviliansService
    {
        // Auth methods
        public Task CreateAsync(RegistrationViewModel regParams);
        public Task<TokensViewModel> LoginAsync(LoginViewModel loginViewModel);

        // Management methods
        public Task BlockAsync(Guid id);
        public Task UnblockAsync(Guid id);
        public Task DeleteAsync(Guid id);
        public Task<Civilian> GetByIdAsync(Guid id);
        public Task<List<Civilian>> GetAllAsync(CiviliansPaginationParametersViewModel pageParams);
        public Task<IList<string>> GetRolesAsync(Guid id);
        public Task ChangeRoleAsync(Guid id, string roleName);
    }
}
