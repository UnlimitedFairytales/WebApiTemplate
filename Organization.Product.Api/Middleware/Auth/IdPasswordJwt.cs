using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Organization.Product.Domain.ValueObjects.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
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

        public void AddSecurityDefinition_AddSecurityRequirement(SwaggerGenOptions options)
        {
            var schemeName = JwtBearerDefaults.AuthenticationScheme;
            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Please input jwt_token"
            });
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                        {
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = schemeName
                                }
                            },
                            Array.Empty<string>()
                        }
                });
        }

        public void UseAuthentication(WebApplication app, IConfiguration configuration)
        {
            app.UseAuthentication();
        }
    }
}
