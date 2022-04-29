using Library.Infrastructure.Crosscutting.Models;
using System.Security.Cryptography;
using System.Text;

namespace Library.Infrastructure.Crosscutting.Helpers
{
    public static class HashSaltHelper
    {
        public static GeneratedHashSalt Salt(string data, string salt = "")
        {
            salt = string.IsNullOrWhiteSpace(salt) ? Guid.NewGuid().ToString("N") : salt;
            var dataWithSalt = $"{data}{salt}";
            using var hash = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            hash.ComputeHash(Encoding.UTF8.GetBytes(dataWithSalt));

            if (hash == null)
            {
                throw new Exception();
            }

            return new(HashConverter(hash.Hash), salt);
        }

        public static bool ValidateSalt(string data, GeneratedHashSalt salted)
        {
            return Salt(data, salted.Salt).Hash == salted.Hash;
        }

        private static string HashConverter(byte[] data)
        {
            return BitConverter.ToString(data).ToString().Replace("-", string.Empty);
        }
    }
}
