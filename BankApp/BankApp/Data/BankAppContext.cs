using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankApp.Models;

namespace BankApp.Data
{
    public class BankAppContext : DbContext
    {
        public BankAppContext (DbContextOptions<BankAppContext> options)
            : base(options)
        {
        }

        public DbSet<BankApp.Models.User> Users { get; set; } = default!;
    }
}
