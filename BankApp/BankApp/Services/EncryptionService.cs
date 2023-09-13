using BankApp.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BankApp.Services
{
    public class EncryptionService : IEncryptionService
    {

        private const byte EncryptionKey = 0x53;

        /// <summary>
        /// Function that will encrypt the input string.
        /// I know this is not real encryption. However, I do not know any encryption algorithm yet.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Hashed string</returns>
        public string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");
            }

            var saltedInput = $"Som3 s@lt?{input}";
            byte[] plainBytes = Encoding.UTF8.GetBytes(saltedInput);
            byte[] encryptedBytes = new byte[plainBytes.Length];

            for (int i = 0; i < plainBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(plainBytes[i] ^ EncryptionKey);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Function that will reverse a hashed value
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");
            }

            byte[] encryptedBytes = Convert.FromBase64String(input);
            byte[] decryptedBytes = new byte[encryptedBytes.Length];

            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ EncryptionKey);
            }

            string saltToRemove = "S0me s@lt?";
            return Encoding.UTF8.GetString(decryptedBytes).Substring(saltToRemove.Length);
        }
    }
}
