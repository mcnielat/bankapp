using BankApp.Models;

namespace BankApp.Interfaces
{
    /// <summary>
    /// Interface to provide abstraction to the actual Transactions implementation
    /// </summary>
    public interface ITransactionsService
    {
        public Task<UserDto> InquireBalance(int accountId, string? pin, string? password);

        public Task<UserDto> WithdrawMoney(Withdrawal withdrawalInfo);

        public Task<UserDto> DepositMoney(Deposit depositInfo);

        public Task<UserDto> TransferMoney(TransferMoney transferMoneyInfo);
    }
}
