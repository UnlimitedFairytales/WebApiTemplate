using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Organization.Product.Api.Utils.Hasher
{
    // 参考
    // https://source.dot.net/#Microsoft.Extensions.Identity.Core/PasswordHasher.cs,f120517802334c3b,references
    public class Pbkdf2Hasher : IHasher
    {
        public string Hash(string text, string salt)
        {
            byte[] salt_bytes = System.Text.Encoding.UTF8.GetBytes(salt);
            if (salt_bytes.Length < 128 / 8)
            {
                throw new Exception("Salt bit length must be 128 bits or more(16 characters or more)");
            }
            var hashed_bytes = KeyDerivation.Pbkdf2(text, salt_bytes, KeyDerivationPrf.HMACSHA512, 100000, 256 / 8); // 256 / 8 = 32bytes = 256bits
            var hashed_str = Convert.ToBase64String(hashed_bytes); // (256/6)'s next multiple of 4  = 44 chars
            return hashed_str;
        }
    }
}