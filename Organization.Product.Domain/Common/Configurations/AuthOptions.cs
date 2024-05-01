using Microsoft.Extensions.Configuration;

namespace Organization.Product.Domain.Common.Configurations
{
    public class AuthOptions
    {
        public const string AuthType_IdPasswordCookie = "IdPasswordCookie";
        public const string AuthType_IdPasswordJwt = "IdPasswordJwt";

        // static
        // ----------------------------------------
        // instance

        public string AuthType { get; set; } = string.Empty;
        public Cookie Cookie { get; set; } = new Cookie();
        public Jwt Jwt { get; set; } = new Jwt();

        public AuthOptions(IConfiguration configuration)
        {
            configuration.GetSection("Auth").Bind(this);
        }
    }

    public class Cookie
    {
        public int Expire_Minute { get; set; }
    }

    public class Jwt
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string IssuerSigningKey { get; set; } = string.Empty;
        public int Expire_Minute { get; set; }
    }
}