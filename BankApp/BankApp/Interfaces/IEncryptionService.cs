using System.Security;

namespace BankApp.Interfaces
{
    public interface IEncryptionService
    {
        public string Encrypt(string input);

        public string Decrypt(string input);
    }
}
