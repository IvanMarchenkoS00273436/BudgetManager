using BudgetManager.DatabaseSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.Models;

namespace BudgetManager.Controllers
{
    public class TransactionController
    {
        private DbContextData _dbContext = new DbContextData();

        // Get method to retrieve all transactions
        public List<Transaction> GetTransactionsByUserId(int userId)
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
                }

                return _dbContext.Transactions.Where(t => t.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TransactionType> GetTransactionsTypes()
        {
            try { return _dbContext.TransactionTypes.ToList(); }
            catch (Exception ex) { throw ex; }
        }
        public List<Transaction> GetFilteredTransactionsByUserId(int userId,
            DateTime transactionDateTime, int minAmount, int transactionTypeId)
        {
            try
            {
                if (userId == null)
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                var query = _dbContext.Transactions.AsQueryable();

                query = query.Where(t => t.UserId == userId);

                if (transactionDateTime != default)
                    query = query.Where(t => t.TransactionsDate == transactionDateTime);

                if (minAmount > 0)
                    query = query.Where(t => t.TransactionAmount >= minAmount);

                if (transactionTypeId > 0)
                    query = query.Where(t => t.TransactionTypeId == transactionTypeId);

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public Transaction GetTransactionById(int transactionId)
        {
            return _dbContext.Transactions.Where(t => t.TransactionId == transactionId).FirstOrDefault();
        }

        public void DeleteTransactionById(int transactionId)
        {
            try
            {
                var transaction = _dbContext.Transactions.Find(transactionId);
                if (transaction != null)
                {
                    _dbContext.Transactions.Remove(transaction);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddTransaction(Transaction transaction)
        {
            try
            {
                // Валидация
                if (transaction.TransactionAmount <= 0)
                    throw new ArgumentException("Amount must be greater than 0");

                if (transaction.TransactionsDate > DateTime.Now)
                    throw new ArgumentException("Date cannot be in the future");

                _dbContext.Transactions.Add(transaction);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding transaction: {ex.Message}");
            }
        }

        public bool EditTransaction(Transaction transaction)
        {
            try
            {
                if (transaction.TransactionAmount <= 0)
                    throw new ArgumentException("Amount must be greater than 0");

                if (transaction.TransactionsDate > DateTime.Now)
                    throw new ArgumentException("Date cannot be in the future");

                var existingTransaction = _dbContext.Transactions.Find(transaction.TransactionId);
                if (existingTransaction == null)
                    throw new ArgumentException("Transaction not found");

                existingTransaction.TransactionTypeId = transaction.TransactionTypeId;
                existingTransaction.TransactionAmount = transaction.TransactionAmount;
                existingTransaction.TransactionsDate = transaction.TransactionsDate;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
