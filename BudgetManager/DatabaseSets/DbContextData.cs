using BudgetManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BudgetManager.DatabaseSets
{
    public class DbContextData : DbContext
    {
        public DbContextData() : base("BudgetManagerDB") { }
        public DbSet<User> Users { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet <Budget> Budgets { get; set; }
        public DbSet <BalanceSnaphot> BalanceSnaphots { get; set; }
    }
}
