using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankApp.Services
{
    /// <summary>
    /// Service that contains the logic for the common banking transactions
    /// </summary>
    public class TransactionsService : ITransactionsService
    {
        private readonly BankAppContext _context;
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// Constructs the TransactionsService class with the database context
        /// </summary>
        /// <param name="context">The context to use for the database connection</param>
        public TransactionsService(BankAppContext context, IEncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
        }

        /// <summary>
        /// Checks the database for the account ID and validates the pin or password to be used for the balance inquiry
        /// </summary>
        /// <param name="accountId">Uinique identifier for the user</param>
        /// <param name="pin">PIN of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>User information with the available balance</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserDto> InquireBalance(int accountId, string? pin, string? password)
        {
            var user = await GetUser(accountId);

            if (user is null)
                throw new ArgumentException("Cannot find user");

            var decryptedUser = DecryptCredentials(user);

            if (decryptedUser.HashedPin != pin &&
                decryptedUser.HashedPassword != password)
                throw new ArgumentException("Wrong PIN or password");

            return new UserDto
            {
                AccountId = accountId,
                UserName = user.UserName,
                Balance = user.Balance,
            };
        }

        /// <summary>
        /// Function to withdraw money for a user's account
        /// </summary>
        /// <param name="withdrawalInfo"></param>
        /// <returns>Updated information of the user after the withdrawal</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserDto> WithdrawMoney(Withdrawal withdrawalInfo)
        {
            var user = await GetUser(withdrawalInfo.AccountId);
            if (user is null)
                throw new ArgumentException("Cannot find user");

            var decryptedUser = DecryptCredentials(user);

            if (decryptedUser.HashedPin != withdrawalInfo.HashedPin &&
                decryptedUser.HashedPassword != withdrawalInfo.HashedPassword)
                throw new ArgumentException("Wrong PIN or password");

            if (user.Balance < withdrawalInfo.Amount)
                throw new ArgumentException("Withdrawal amount is greater than account balance");

            user.Balance -= withdrawalInfo.Amount;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new UserDto
            {
                AccountId = user.AccountId,
                UserName = user.UserName,
                Balance = user.Balance,
            }; 
        }

        /// <summary>
        /// Function to deposit money into a user's account
        /// </summary>
        /// <param name="depositInfo"></param>
        /// <returns>Updated information of the user after the deposit</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserDto> DepositMoney(Deposit depositInfo)
        {
            var user = await GetUser(depositInfo.AccountId);
            if (user is null)
                throw new ArgumentException("Cannot find user");

            if (!string.IsNullOrEmpty(depositInfo.UserName) && user.UserName != depositInfo.UserName)
                throw new ArgumentException("Account Id does not match username");

            user.Balance += depositInfo.Amount;

            _context.Entry(user).State = EntityState.Modified;

             await _context.SaveChangesAsync();

            return new UserDto
            {
                AccountId = user.AccountId,
                UserName = user.UserName,
                Balance = user.Balance,
            };
        }

        /// <summary>
        /// Function to deposit money from one user to another
        /// </summary>
        /// <param name="transferMoneyInfo"></param>
        /// <returns>Updated information of the source account</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserDto> TransferMoney(TransferMoney transferMoneyInfo)
        {
            var sourceUser = await GetUser(transferMoneyInfo.SourceAccountId);
            if (sourceUser is null)
                throw new ArgumentException("Cannot find source account");

            var destinationUser = await GetUser(transferMoneyInfo.DestinationAccountId);
            if (destinationUser is null)
                throw new ArgumentException("Cannot find destination account");

            var decryptedUser = DecryptCredentials(sourceUser);

            if (decryptedUser.HashedPin != transferMoneyInfo.HashedPin &&
                decryptedUser.HashedPassword != transferMoneyInfo.HashedPassword)
                throw new ArgumentException("Wrong PIN or password");

            if (sourceUser.Balance < transferMoneyInfo.Amount)
                throw new ArgumentException("Transfer amount is greater than account balance");

            sourceUser.Balance -= transferMoneyInfo.Amount;
            destinationUser.Balance += transferMoneyInfo.Amount;

            _context.Entry(sourceUser).State = EntityState.Modified;
            _context.Entry(destinationUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new UserDto
            {
                AccountId = sourceUser.AccountId,
                UserName = sourceUser.UserName,
                Balance = sourceUser.Balance,
            };
        }

        /// <summary>
        /// Function to get a user from the context using the accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>User information that matches the account Id</returns>
        private async Task<User?> GetUser(int accountId)
        {
            if (_context.Users == null)
            {
                return null;
            }
            var user = await _context.Users.FindAsync(accountId);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        private User DecryptCredentials(User user)
        {
            var decryptedPin = "";
            var decryptedPw = "";

            if (!string.IsNullOrEmpty(user.HashedPin))
                decryptedPin = _encryptionService.Decrypt(user.HashedPin);

            if (!string.IsNullOrEmpty(user.HashedPassword))
                decryptedPw = _encryptionService.Decrypt(user.HashedPassword);

            return new User
            {
                HashedPassword = decryptedPw,
                HashedPin = decryptedPin
            };
        }

    }
}
