﻿using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Tokens;
using Civilians.Core.Auth;
using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLib.ExceptionsHandler.CustomExceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Civilians.Application.Services
{
    public class TokensService : ITokensService
    {
        private readonly JwtConfigModel _jwtConfig;
        private readonly IUnitOfWork _unitOfWork;

        public TokensService(IOptions<JwtConfigModel> jwtConfig, IUnitOfWork unitOfWork)
        {
            _jwtConfig = jwtConfig != null ? jwtConfig.Value : throw new ArgumentNullException(nameof(jwtConfig));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public JwtSecurityToken CreateAccessToken(IList<Claim> claims)
        {
            var symSecurityKey = _jwtConfig.Key;
            return new(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                notBefore: DateTime.UtcNow,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AuthTokenLifetimeInMinutes),
                signingCredentials: new SigningCredentials(symSecurityKey, _jwtConfig.SecurityAlgorithm));
        }

        public async Task<RefreshToken> IssueRefreshTokenAsync(Guid userId)
        {
            var existingRefreshToken = await _unitOfWork.TokensRepository.GetByUserIdAsync(userId);
            var newRefreshToken = GenerateRefreshToken(userId);

            if (existingRefreshToken == null)
            {
                newRefreshToken = CreateNewRefreshToken(newRefreshToken);
            }
            else
            {
                newRefreshToken = UpdateExistingRefreshToken(existingRefreshToken, newRefreshToken);
            }

            await _unitOfWork.SaveChangesAsync();
            return newRefreshToken;
        }

        private RefreshToken UpdateExistingRefreshToken(RefreshToken existingRefreshToken, RefreshToken newRefreshToken)
        {
            existingRefreshToken.ExpiresAt = newRefreshToken.ExpiresAt;
            existingRefreshToken.IssuedAt = newRefreshToken.IssuedAt;
            existingRefreshToken.IsRevoked = false;
            _unitOfWork.TokensRepository.UpdateExistingRefreshToken(existingRefreshToken);

            return existingRefreshToken;
        }

        private RefreshToken CreateNewRefreshToken(RefreshToken newRefreshToken)
        {
            _unitOfWork.TokensRepository.IssueNewRefreshToken(newRefreshToken);

            return newRefreshToken;
        }

        private RefreshToken GenerateRefreshToken(Guid userId)
        {
            var expiresAt = _jwtConfig.RefreshTokenLifetimeInDays;

            return new RefreshToken(DateTime.UtcNow.AddDays(expiresAt), userId);
        }

        public async Task<TokensViewModel> RefreshAccessTokenAsync(TokensRefreshingViewModel tokensPairViewModel)
        {
            var principal = GetClaimsPrincipalFromToken(tokensPairViewModel.AccessToken);
            if (principal == null)
            {
                throw new BadRequestException("Invalid access token or refresh token.");
            }

            var user = await FindUserByClaimsAsync(principal);

            ValidateRefreshToken(user.RefreshToken, tokensPairViewModel.RefreshToken);

            var newRefreshToken = await IssueRefreshTokenAsync(user.Id);

            var newAccessToken = CreateAccessToken(principal.Claims.ToList());

            return new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt,
                AccessTokenExpiresAt = newAccessToken.ValidTo
            };
        }

        private ClaimsPrincipal? GetClaimsPrincipalFromToken(string accessToken)
        {
            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtConfig.Key,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out SecurityToken securityToken);
            
            // Checking if the validated token is a JWT
            // Then checking whether the token signature algorithm is correct
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(_jwtConfig.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid access token or refresh token.");
            }

            return principal;
        }

        private async Task<User> FindUserByClaimsAsync(ClaimsPrincipal claimsPrincipal)
        {
            string? userIdAsString = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdAsString))
            {
                throw new ArgumentException("Couldn't get the user's id from the claims.");
            }
            
            if(!Guid.TryParse(userIdAsString, out var userId))
            {
                throw new BadRequestException("Couldn't parse a userId");
            }

            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("There is no user with the specified id.");
            }

            return user;
        }

        private static void ValidateRefreshToken(RefreshToken? storedRefreshToken, Guid providedRefreshToken)
        {
            if (storedRefreshToken == null)
            {
                throw new BadRequestException("User's refresh token is null.");
            }

            if (!storedRefreshToken.IsActive)
            {
                throw new BadRequestException("User's refresh token is expired or revoked.");
            }

            if (!storedRefreshToken.Token.Equals(providedRefreshToken))
            {
                throw new BadRequestException("User's refresh token doesn't equal to the provided refresh token.");
            }
        }

        public async Task RevokeAllRefreshTokensAsync() 
            => await _unitOfWork.TokensRepository.RevokeAllRefreshTokensAsync();

        public async Task RevokeRefreshTokenAsync(Guid userId)
        {
            var refreshToken = await _unitOfWork.TokensRepository.GetByUserIdAsync(userId);
            if (refreshToken is null)
            {
                throw new NotFoundException("Refresh token with specified user id hasn't been found.");
            }

            _unitOfWork.TokensRepository.RevokeRefreshToken(refreshToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
