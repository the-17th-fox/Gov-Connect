using Civilians.Core.Models;
using Civilians.Core.ViewModels.Civilians;
using Civilians.Core.ViewModels.Tokens;

namespace Civilians.Application.Interfaces
{
    public interface IUsersService
    {
        // Auth methods
        public Task CreateAsync(RegistrationViewModel registrationsParams);
        public Task<TokensViewModel> LoginAsync(LoginViewModel loginViewModel);

        // Management methods
        public Task BlockAsync(Guid id);
        public Task UnblockAsync(Guid id);
        public Task<User> GetByIdAsync(Guid id);
        public Task<List<User>> GetAllAsync(UsersPaginationParametersViewModel pageParams);
        public Task<IList<string>> GetRolesAsync(User user);
        public Task ChangeRoleAsync(Guid id, string roleName);
    }
}
