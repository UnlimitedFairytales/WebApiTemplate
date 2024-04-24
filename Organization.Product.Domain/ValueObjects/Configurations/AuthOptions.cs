using Microsoft.Extensions.Configuration;

namespace Organization.Product.Domain.ValueObjects.Configurations
{
    public class AuthOptions
    {
        public const string AuthType_IdPasswordCookie = "IdPasswordCookie";
        public const string AuthType_IdPasswordJwt = "IdPasswordJwt";

        public static AuthOptions Load(IConfiguration configuration)
        {
            var obj = new AuthOptions();
            configuration.GetSection("Auth").Bind(obj);
            return obj;
        }

        public string AuthType { get; set; } = String.Empty;
        public Cookie Cookie { get; set; } = new Cookie();
        public Jwt Jwt { get; set; } = new Jwt();
    }

    public class Cookie
    {
        public int Expire_Minute { get; set; }
    }

    public class Jwt
    {
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public string IssuerSigningKey { get; set; } = String.Empty;
        public int Expire_Minute { get; set; }
    }
}