using BudgetManager.DatabaseSets;
using BudgetManager.Models;

namespace BudgetManager.Controllers
{
    public class TransactionController
    {
        private DbContextData _dbContext = new DbContextData();
        private readonly BudgetsController _budgetsController = new BudgetsController();

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

        // Get transaction by id
        public Transaction GetTransactionById(int transactionId)
        {
            return _dbContext.Transactions.Where(t => t.TransactionId == transactionId).FirstOrDefault();
        }

        // Delete transaction by id
        public void DeleteTransactionById(int transactionId)
        {
            try
            {
                var transaction = _dbContext.Transactions.Find(transactionId);
                if (transaction != null)
                {
                    // Decrease the spent amount in the corresponding budget
                    _budgetsController.UpdateBudgetSpentAmount(
                        transaction.UserId,
                        transaction.TransactionTypeId,
                        -transaction.TransactionAmount);

                    // Delete the transaction
                    _dbContext.Transactions.Remove(transaction);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Add transaction
        public bool AddTransaction(Transaction transaction)
        {
            try
            {
                // Validation
                if (transaction.TransactionAmount <= 0)
                    throw new ArgumentException("Amount must be greater than 0");

                if (transaction.TransactionsDate > DateTime.Now)
                    throw new ArgumentException("Date cannot be in the future");

                _dbContext.Transactions.Add(transaction);
                _dbContext.SaveChanges();

                _budgetsController.UpdateBudgetSpentAmount(
                transaction.UserId,
                transaction.TransactionTypeId,
                transaction.TransactionAmount);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding transaction: {ex.Message}");
            }
        }

        // Edit transaction
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

                // Calculate the difference in amount if transaction type is the same
                decimal amountDifference = 0;
                if (existingTransaction.TransactionTypeId == transaction.TransactionTypeId)
                {
                    amountDifference = transaction.TransactionAmount - existingTransaction.TransactionAmount;
                }
                else
                {
                    // If transaction type changed, subtract the old amount from old type's budget
                    _budgetsController.UpdateBudgetSpentAmount(
                        transaction.UserId,
                        existingTransaction.TransactionTypeId,
                        -existingTransaction.TransactionAmount);

                    // And add the new amount to the new type's budget
                    amountDifference = transaction.TransactionAmount;
                }

                // Update the transaction
                existingTransaction.TransactionTypeId = transaction.TransactionTypeId;
                existingTransaction.TransactionAmount = transaction.TransactionAmount;
                existingTransaction.TransactionsDate = transaction.TransactionsDate;
                _dbContext.SaveChanges();

                // Update the budget for the new/modified transaction amount
                if (amountDifference != 0)
                {
                    _budgetsController.UpdateBudgetSpentAmount(
                        transaction.UserId,
                        transaction.TransactionTypeId,
                        amountDifference);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}