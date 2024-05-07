namespace Organization.Product.Shared.Utils.Hasher
{
    public interface IHasher
    {
        string Hash(string text, string salt);
    }
}
