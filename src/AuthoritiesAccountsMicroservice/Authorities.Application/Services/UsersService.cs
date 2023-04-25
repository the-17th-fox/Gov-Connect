using Authorities.Application.Interfaces;
using Authorities.Application.ViewModels.Tokens;
using Authorities.Core.Interfaces;
using Authorities.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authorities.Application.ViewModels.Pagination;
using Authorities.Application.ViewModels.Users;

namespace Authorities.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokensService _tokensService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(UserManager<User> userManager, ITokensService tokensService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Auth section bellow

        public async Task CreateAsync(RegistrationViewModel registrationParams)
        {
            var user = new User()
            {
                UserName = registrationParams.Email,
                PhoneNumber = registrationParams.PhoneNumber,
                Email = registrationParams.Email
            };

            var result = await _userManager.CreateAsync(user, registrationParams.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + result.Errors.First<IdentityError>().Description);
            }
        }

        public async Task<TokensViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _unitOfWork.UsersRepository.GetByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("There is no user with the specified email.");
            }

            if (user.IsDeleted || user.LockoutEnabled)
            {
                throw new ArgumentException("User is deleted or blocked.");
            }

            var isCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!isCorrect)
            {
                throw new UnauthorizedAccessException("Password check has been failed.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = GetClaims(user, userRoles);

            var accessToken = _tokensService.CreateAccessToken(userClaims);
            var refreshToken = await _tokensService.IssueRefreshTokenAsync(user.Id);

            return new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                AccessTokenExpiresAt = accessToken.ValidTo
            };
        }

        private static List<Claim> GetClaims(User user, IList<string> userRoles)
        {
            var claims = new List<Claim>()
            {
                new Claim("uid", user.Id.ToString()),
                new Claim("identifyas", "civilians")
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        // Users management section bellow

        public async Task<List<User>> GetAllAsync(UsersPaginationParametersViewModel pageParams)
        {
            return await _unitOfWork.UsersRepository.GetAllAsync(
                pageParams.PageNumber,
                pageParams.PageSize,
                pageParams.ShowDeleted,
                pageParams.ShowBlocked);
        }

        public async Task<List<User>> GetNotConfirmedAsync(PaginationParametersViewModel pageParams)
        {
            return await _unitOfWork.UsersRepository.GetNotConfirmedAsync(
                pageParams.PageNumber, 
                pageParams.PageSize);
        }

        public async Task<User> GetByIdAsync(Guid id)
            => await GetIfExistsAsync(id);

        public async Task<IList<string>> GetRolesAsync(User user)
            => await _userManager.GetRolesAsync(user);

        public async Task ChangeRolesAsync(Guid id, string roleName)
        {
            var user = await GetIfExistsAsync(id);

            if (user.IsDeleted || user.LockoutEnabled)
            {
                throw new ArgumentException("User is deleted or blocked.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            await RemoveRolesAsync(user, userRoles);

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Adding to role failed: " + result.Errors.First<IdentityError>().Description);
            }

            if (!user.IsConfirmed)
            {
                await ChangeConfirmedStatusAsync(user, isConfirmed: true);
            }
        }

        /// <summary>
        /// Removes user's roles with changing the IsConfirmed status to False.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task RemoveRolesAsync(Guid id)
        {
            var user = await GetIfExistsAsync(id);

            if (user.IsDeleted || user.LockoutEnabled)
            {
                throw new ArgumentException("User is deleted or blocked.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            await RemoveRolesAsync(user, userRoles);

            await ChangeConfirmedStatusAsync(user, isConfirmed: false);
        }

        /// <summary>
        /// Removes all user's roles without changing user's confirmation status.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task RemoveRolesAsync(User user, IList<string> roles)
        {
            if (!roles.Any())
            {
                return;
            }

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                throw new Exception("Removing from role failed: " + result.Errors.First<IdentityError>().Description);
            }
        }

        public async Task BlockAsync(Guid id)
        {
            var user = await GetIfExistsAsync(id);

            if (user.LockoutEnabled)
            {
                throw new ArgumentException("User has been already blocked.");
            }

            user.IsBlocked = true;

            await UpdateAsync(user);
        }

        public async Task UnblockAsync(Guid id)
        {
            var user = await GetIfExistsAsync(id);

            if (!user.LockoutEnabled)
            {
                throw new ArgumentException("User isn't blocked.");
            }

            user.IsBlocked = false;

            await UpdateAsync(user);
        }
		
        private async Task<User> GetIfExistsAsync(Guid id)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User with the specified id couldn't been found.");
            }

            return user;
        }

        private async Task ChangeConfirmedStatusAsync(User user, bool isConfirmed)
        {
            user.IsConfirmed = isConfirmed;

            await UpdateAsync(user);
        }
		
        private async Task UpdateAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("User updating has failed: " + result.Errors.First<IdentityError>().Description);
            }
        }
    }
}
