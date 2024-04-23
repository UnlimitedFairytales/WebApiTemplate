namespace Organization.Product.Api.Middleware.Auth
{
    public interface IAuthMethods
    {
        void AddAuthentication(IServiceCollection services, IConfiguration configuration);
        void UseAuthentication(WebApplication app, IConfiguration configuration);
    }
}
