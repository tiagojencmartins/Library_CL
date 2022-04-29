using System.Security.Cryptography;
using System.Text;

namespace Library.Infrastructure.Crosscutting.Helpers
{
    public class EncryptionHelper
    {
        public static class EncryptionService
        {
            private static string _key = "b14ca5898a4e4133bbce2ea2315a1916";

            public static string Decrypt(string data)
            {
                var vector = new byte[16];
                var dataAsString = Convert.FromBase64String(data);

                using Aes aes = Aes.Create();

                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.IV = vector;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new MemoryStream(dataAsString);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);
                return streamReader.ReadToEnd();
            }

            public static string Encrypt(string data)
            {
                var vector = new byte[16];
                byte[] encryptedData;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(_key);
                    aes.IV = vector;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new MemoryStream();
                    using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new(cryptoStream))
                    {
                        streamWriter.Write(data);
                    }

                    encryptedData = memoryStream.ToArray();
                }

                return Convert.ToBase64String(encryptedData);
            }
        }
    }
}
