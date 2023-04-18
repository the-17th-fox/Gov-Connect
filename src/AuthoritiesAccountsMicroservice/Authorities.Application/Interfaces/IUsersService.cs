using Authorities.Application.ViewModels.Pagination;
using Authorities.Application.ViewModels.Tokens;
using Authorities.Application.ViewModels.Users;
using Authorities.Core.Models;

namespace Authorities.Application.Interfaces
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
        public Task ChangeRolesAsync(Guid id, string roleName);
        public Task RemoveRolesAsync(Guid id);
        public Task<List<User>> GetNotConfirmedAsync(PaginationParametersViewModel pageParams);
    }
}
