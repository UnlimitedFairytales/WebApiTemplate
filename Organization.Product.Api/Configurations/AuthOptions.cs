namespace Organization.Product.Api.Configurations
{
    public class AuthOptions
    {
        public const string AuthType_IdPasswordCookie = "IdPasswordCookie";
        public const string AuthType_IdPasswordJwt = "IdPasswordJwt";
        public const string AuthType_IdPasswordSession = "IdPasswordSession";

        // static
        // ----------------------------------------
        // instance

        public string AuthType { get; set; } = string.Empty;
        public Cookie Cookie { get; set; } = new Cookie();
        public Jwt Jwt { get; set; } = new Jwt();
        public Session Session { get; set; } = new Session();

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

    public class Session
    {
        public const string StoreType_InMemory = "InMemory";
        public const string StoreType_Redis = "Redis";

        // static
        // ----------------------------------------
        // instance

        public int IdleTimeout_Minute { get; set; }
        public string StoreType { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
    }
}