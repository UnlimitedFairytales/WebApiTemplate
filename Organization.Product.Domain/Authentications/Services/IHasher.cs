namespace Organization.Product.Domain.Authentications.Services
{
    public interface IHasher
    {
        string Hash(string text, string salt);
    }
}
