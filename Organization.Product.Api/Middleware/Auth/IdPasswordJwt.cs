using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Organization.Product.Domain.ValueObjects.Configurations;
using System.Text;

namespace Organization.Product.Api.Middleware.Auth
{
    public class IdPasswordJwt : IAuthMethods
    {
        public void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = AuthOptions.Load(configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidIssuer = authOptions.Jwt.Issuer,
                        ValidAudience = authOptions.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Jwt.IssuerSigningKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
                });
        }

        public void UseAuthentication(WebApplication app, IConfiguration configuration)
        {
            app.UseAuthentication();
        }
    }
}
