namespace Organization.Product.Gateways.Authentications
{
    public interface IHasher
    {
        string Hash(string text, string salt);
    }
}
