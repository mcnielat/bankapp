using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Services
{
    /// <summary>
    /// Service that contains the logic for user-related actions
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly BankAppContext _context;
        private readonly IEncryptionService _encryptionService;
        public UsersService(BankAppContext context,
                            IEncryptionService encryptionService)
        {

            _context = context;
            _encryptionService = encryptionService;
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The updated information of the user with the generated Id</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<User> AddUser(User user)
        {
            if (_context.Users == null)
                throw new ArgumentException("Cannot create context properly");

            EncryptCredentials(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        /// <summary>
        /// Encrypts PIN and password of the user
        /// </summary>
        /// <param name="user"></param>
        private void EncryptCredentials(User user)
        {
            if (!string.IsNullOrEmpty(user.HashedPin))
                user.HashedPin = _encryptionService.Encrypt(user.HashedPin);

            if (!string.IsNullOrEmpty(user.HashedPassword))
                user.HashedPassword = _encryptionService.Encrypt(user.HashedPassword);
        }
    }
}
