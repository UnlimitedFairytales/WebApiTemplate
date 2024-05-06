namespace Organization.Product.Gateway.Authentications
{
    public interface IHasher
    {
        string Hash(string text, string salt);
    }
}
