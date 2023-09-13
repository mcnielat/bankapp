namespace BankApp.Models
{
    public class Deposit
    {
        public int AccountId { get; set; }
        public string? UserName { get; set; }
        public double Amount { get; set; }

        public bool Validate()
        {
            return AccountId > 0;
        }
    }
}
