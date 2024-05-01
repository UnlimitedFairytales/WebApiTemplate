using Microsoft.IdentityModel.Tokens;
using Organization.Product.Domain.Common.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Organization.Product.ApplicationServices.UseCases.Login
{
    public class JwtAuthService
    {
        static readonly DateTime UNIX_EPOCH = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        static long GetUnixTime(DateTime datetime)
        {
            var span = datetime.ToUniversalTime() - UNIX_EPOCH;
            return (long)span.TotalSeconds;
        }

        public static LoginResultDto SignIn(LoginRequestDto requestDto, AuthOptions authOptions)
        {
            var token = GenerateToken(
                authOptions.Jwt.Issuer,
                authOptions.Jwt.Audience,
                requestDto.UserCd!,
                DateTime.UtcNow,
                Guid.NewGuid().ToString("N"),
                authOptions.Jwt.Expire_Minute,
                authOptions.Jwt.IssuerSigningKey);
            return new LoginResultDto() { Token = token };
        }

        public static string GenerateToken(
            string issuer,
            string audience,
            string subject,
            DateTime issuedAt,
            string jwtId,
            int expire_minute,
            string secretKey)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Iat, GetUnixTime(issuedAt).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId)
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
    }
}
