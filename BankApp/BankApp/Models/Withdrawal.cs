using Microsoft.Identity.Client;

namespace BankApp.Models
{
    public class Withdrawal
    {
        public int AccountId { get; set; }
        public string? HashedPin { get; set; }
        public string? HashedPassword { get; set; }
        public double Amount { get; set; }

        public bool Validate()
        {
            if (AccountId <= 0)
                throw new ArgumentException("Account ID cannot be empty");

            if (string.IsNullOrEmpty(HashedPin) && string.IsNullOrEmpty(HashedPassword))
                throw new ArgumentException("PIN and password cannot be empty at the same time");

            return true;
        }
    }
}
