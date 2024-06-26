﻿using Microsoft.IdentityModel.Tokens;
using Organization.Product.Domain.Authentications.Entities;
using Organization.Product.Domain.Authentications.Repositories;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Authentications.ValueObjects;
using Organization.Product.Domain.Common.ValueObjects;
using Organization.Product.Shared.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Organization.Product.Api._1_Middleware.Auth.Jwt
{
    public class JwtAppAuthenticationService : IAppAuthenticationService
    {
        public static readonly string VER = "ver";

        static readonly DateTime UNIX_EPOCH = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        static long GetUnixTime(DateTime datetime)
        {
            var span = datetime.ToUniversalTime() - UNIX_EPOCH;
            return (long)span.TotalSeconds;
        }

        static string GenerateToken(
            string issuer,
            string audience,
            string subject,
            DateTime issuedAt,
            string jwtId,
            int expire_minute,
            string secretKey,
            AppAuthenticatedUser user)
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, subject),
                new(JwtRegisteredClaimNames.Iat, GetUnixTime(issuedAt).ToString()),
                new(JwtRegisteredClaimNames.Jti, jwtId),
                // private claim
                new(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(VER,  (user.Ver ?? 0).ToString()),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                null,
                DateTime.UtcNow.AddMinutes(expire_minute),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // static
        // ----------------------------------------
        // instance

        readonly AuthOptions _authOptions;
        readonly IAppAuthenticatedUserRepository _appAuthenticatedUserRepository;

        public JwtAppAuthenticationService(AuthOptions authOptions, IAppAuthenticatedUserRepository appAuthenticatedUserRepository)
        {
            this._authOptions = authOptions;
            this._appAuthenticatedUserRepository = appAuthenticatedUserRepository;
        }

        public async Task<AppAuthenticationResult> AuthenticateAsync(string userCd, string? password)
        {
            var user = this._appAuthenticatedUserRepository.FindBy(userCd, password!);
            var token = GenerateToken(
                this._authOptions.Jwt.Issuer,
                this._authOptions.Jwt.Audience,
                user.UserCd!,
                DateTime.UtcNow,
                Guid.NewGuid().ToString("N"),
                this._authOptions.Jwt.Expire_Minute,
                this._authOptions.Jwt.IssuerSigningKey,
                user);
            return await Task.FromResult(new AppAuthenticationResult { Token = token });
        }

        public Task SignOutAsync()
        {
            throw AppException.Create(AppMessage.W5006());
        }
    }
}
