using Organization.Product.Domain.Authentications.ValueObjects;

namespace Organization.Product.Domain.Authentications.Services
{
    public interface IAppAuthenticationService
    {
        Task<AppAuthenticationResult> AuthenticateAsync(string userCd, string? password);
        Task SignOutAsync();
    }
}
