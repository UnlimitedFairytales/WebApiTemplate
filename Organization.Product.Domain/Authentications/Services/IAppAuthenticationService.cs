using Organization.Product.Domain.Authentications.ValueObjects;

namespace Organization.Product.Domain.Authentications.Services
{
    public interface IAppAuthenticationService
    {
        AppAuthenticationResult Authenticate(string userCd, string? password);
        void SignOut();
    }
}
