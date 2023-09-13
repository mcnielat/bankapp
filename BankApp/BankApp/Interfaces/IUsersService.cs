using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IUsersService
    {
        public Task<User> AddUser(User user);
    }
}
