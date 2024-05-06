using Organization.Product.Domain.Authentications.Entities;

namespace Organization.Product.Domain.Authentications.Repositories
{
    public interface IAppAuthenticatedUserRepository
    {
        public AppAuthenticatedUser FindBy(string userCd, string password);
    }
}
