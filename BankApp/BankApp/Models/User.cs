using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class User
    {
        [Key]
        public int AccountId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// Password coming from client is expected to be hashed in some way
        /// </summary>
        public string HashedPassword { get; set; }
        /// <summary>
        /// PIN coming from client is expected to be hashed in some way
        /// </summary>
        public string HashedPin { get; set; }
        public double Balance { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(HashedPassword) && string.IsNullOrEmpty(HashedPin))
                throw new ArgumentException("PIN and password cannot be empty at the same time.");

            return true;
        }
    }
}
