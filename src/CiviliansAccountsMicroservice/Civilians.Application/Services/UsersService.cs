using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Civilians;
using Civilians.Application.ViewModels.Tokens;
using Civilians.Core.Auth;
using Civilians.Core.Interfaces;
using Civilians.Core.Misc;
using Civilians.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Civilians.Application.Services
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
            if (registrationParams.PassportRegionCode == RegionCodes.Undefined)
                throw new ArgumentException("Region code is undefined.");

            var user = new User()
            {
                UserName = registrationParams.Email,
                PhoneNumber = registrationParams.PhoneNumber,
                Email = registrationParams.Email
            };

            string regionCode = Enum.GetName<RegionCodes>(registrationParams.PassportRegionCode) 
                ?? throw new Exception("Can not get the name of the region");

            var passport = new Passport()
            {
                User = user,
                FirstName = registrationParams.FirstName.ToUpperInvariant(),
                LastName = registrationParams.LastName.ToUpperInvariant(),
                Patronymic = registrationParams.Patronymic.ToUpperInvariant(),
                Region = regionCode,
                Number = registrationParams.PassportNumber,
            };

            _unitOfWork.PassportsRepository.Create(passport);

            var result = await _userManager.CreateAsync(user, registrationParams.Password);
            if (!result.Succeeded)
                throw new Exception("User creation failed: " + result.Errors.First<IdentityError>().Description);

            result = await _userManager.AddToRoleAsync(user, AuthRoles.DefaultUser);
            if (!result.Succeeded)
                throw new Exception("Adding to role failed: " + result.Errors.First<IdentityError>().Description);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TokensViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _unitOfWork.UsersRepository.GetByEmailAsync(loginViewModel.Email);
            if (user == null)
                throw new KeyNotFoundException("There is no user with the specified email.");

            if (user.IsDeleted || user.LockoutEnabled)
                throw new ArgumentException("User is deleted or blocked.");

            var isCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!isCorrect)
                throw new UnauthorizedAccessException("Password check has been failed.");

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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
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

        public async Task<User> GetByIdAsync(Guid id) 
            => await GetIfExistsAsync(id);

        public async Task<IList<string>> GetRolesAsync(User user) 
            => await _userManager.GetRolesAsync(user);

        private async Task<User> GetIfExistsAsync(Guid id)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("There is no user with the specified id.");

            return user;
        }

        public async Task ChangeRoleAsync(Guid id, string roleName)
        {
            var user = await GetIfExistsAsync(id);

            if (user.IsDeleted || user.LockoutEnabled)
                throw new ArgumentException("User is deleted or blocked.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!result.Succeeded)
                throw new Exception("Removing from role failed: " + result.Errors.First<IdentityError>().Description);

            result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception("Adding to role failed: " + result.Errors.First<IdentityError>().Description);
        }
        
        public async Task BlockAsync(Guid id)
        {
            var user = await GetIfExistsAsync(id);

            if (user.LockoutEnabled)
                throw new ArgumentException("User has been already blocked.");

            user.LockoutEnabled = true;

            await _unitOfWork.UsersRepository.UpdateAsync(user);
        }

        public async Task UnblockAsync(Guid id)
        {
            var user = await GetIfExistsAsync(id);

            if (!user.LockoutEnabled)
                throw new ArgumentException("User isn't blocked.");

            user.LockoutEnabled = false;

            await _unitOfWork.UsersRepository.UpdateAsync(user);
        }
    }
}
