using Microsoft.Identity.Client;

namespace BankApp.Models
{
    public class TransferMoney
    {
        public int SourceAccountId { get; set; }
        public string? HashedPassword { get; set; }
        public string? HashedPin { get; set; }
        public int DestinationAccountId { get; set; }
        public double Amount { get; set; }


        public bool Validate()
        {
            if (SourceAccountId <= 0)
                throw new ArgumentException("Source Account ID cannot be empty");

            if (DestinationAccountId <= 0)
                throw new ArgumentException("Destination Account ID cannot be empty");

            if (SourceAccountId == DestinationAccountId)
                throw new ArgumentException("Source Account ID cannot be the same as Destination Account ID");

            if (string.IsNullOrEmpty(HashedPin) && string.IsNullOrEmpty(HashedPassword))
                throw new ArgumentException("PIN and password cannot be empty at the same time");

            return true;
        }
    }
}
