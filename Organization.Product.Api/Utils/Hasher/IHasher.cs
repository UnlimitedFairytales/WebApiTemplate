using Organization.Product.Domain.Common.ValueObjects;

namespace Organization.Product.Api.Utils.Hasher
{
    public interface IHasher
    {
        string Hash(string text, string salt);

        public static void Validate(string userCd, string password, IHasher hasher)
        {
            var salt = "this is salt3456";
            var hashed = hasher.Hash(password, salt);
            var isValid = userCd == "USER00" && hashed == hasher.Hash("USER00", salt);
            if (!isValid) throw AppException.Create(AppMessage.W5001());
        }
    }
}
