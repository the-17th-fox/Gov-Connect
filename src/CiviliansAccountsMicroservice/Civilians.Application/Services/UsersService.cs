﻿using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Civilians;
using Civilians.Application.ViewModels.Tokens;
using Civilians.Core.Auth;
using Civilians.Core.Interfaces;
using Civilians.Core.Misc;
using Civilians.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedLib.ExceptionsHandler.CustomExceptions;
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
            {
                throw new BadRequestException("Region code is undefined.");
            }

            var user = new User()
            {
                UserName = registrationParams.Email,
                PhoneNumber = registrationParams.PhoneNumber,
                Email = registrationParams.Email
            };

            var passport = new Passport()
            {
                User = user,
                FirstName = registrationParams.FirstName.ToUpperInvariant(),
                LastName = registrationParams.LastName.ToUpperInvariant(),
                Patronymic = registrationParams.Patronymic.ToUpperInvariant(),
                Region = registrationParams.PassportRegionCode,
                Number = registrationParams.PassportNumber,
            };

            _unitOfWork.PassportsRepository.Create(passport);

            var result = await _userManager.CreateAsync(user, registrationParams.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + result.Errors.First<IdentityError>().Description);
            }

            result = await _userManager.AddToRoleAsync(user, AuthRoles.DefaultUser);
            if (!result.Succeeded)
            {
                throw new Exception("Adding to role failed: " + result.Errors.First<IdentityError>().Description);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TokensViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _unitOfWork.UsersRepository.GetByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("There is no user with the specified email.");
            }

            CheckIsBlockedOrDeleted(user);

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
            var policyGroup = DefinePolicyGroup(userRoles);

            var claims = new List<Claim>()
            {
                new Claim("uid", user.Id.ToString()),
                new Claim("fname", user.Passport.FirstName),
                new Claim("pname", user.Passport.Patronymic),
                new Claim("identifyas", "civilians"),
                new Claim("policygroup", policyGroup.ToLowerInvariant())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim("userrole", role));
            }

            return claims;
        }

        private static string DefinePolicyGroup(IList<string> userRoles)
        {
            string? policyGroup = null;

            var isAdminsPolicy = AuthPolicies.Administrators
                .Any(adminsRole => userRoles
                    .Any(userRole => adminsRole == userRole));

            if (isAdminsPolicy)
            {
                return nameof(AuthPolicies.Administrators);
            }

            var isDefaultRightsPolicy = AuthPolicies.DefaultRights
                .Any(defaultPolicy => userRoles
                    .Any(userRole => defaultPolicy == userRole));

            if (isDefaultRightsPolicy)
            {
                return nameof(AuthPolicies.DefaultRights);
            }

            return policyGroup ??= "Undefined";
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

        public async Task ChangeRoleAsync(Guid id, string roleName)
        {
            var user = await GetIfExistsAsync(id);

            CheckIsBlockedOrDeleted(user);

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!result.Succeeded)
            {
                throw new Exception("Removing from role failed: " + result.Errors.First<IdentityError>().Description);
            }

            result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Adding to role failed: " + result.Errors.First<IdentityError>().Description);
            }
        }
        
        public async Task BlockAsync(Guid id)
        {
            var user = await GetIfExistsAsync(id);

            if (user.LockoutEnabled)
            {
                throw new BadRequestException("User has been already blocked.");
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
                throw new NotFoundException("User with the specified id couldn't been found.");
            }

            return user;
        }

        private async Task UpdateAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("User updating has failed: " + result.Errors.First<IdentityError>().Description);
            }
        }

        private static void CheckIsBlockedOrDeleted(User user)
        {
            if (user.IsDeleted || user.LockoutEnabled)
            {
                throw new BadRequestException("User is deleted or blocked.");
            }
        }
    }
}
