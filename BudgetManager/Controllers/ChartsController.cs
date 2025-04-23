using BudgetManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManager.Controllers
{
    public class ChartController
    {
        private TransactionController _transactionController;

        public ChartController(TransactionController transactionController)
        {
            _transactionController = transactionController;
        }

        // Get transaction data for last 7 days
        public Dictionary<DateTime, decimal> GetTransactionsForLast7Days(int userId)
        {
            var result = new Dictionary<DateTime, decimal>();
            var allTransactions = _transactionController.GetTransactionsByUserId(userId);

            // Get the last 7 days
            for (int i = 6; i >= 0; i--)
            {
                DateTime date = DateTime.Today.AddDays(-i);
                decimal dailySum = allTransactions
                    .Where(t => t.TransactionsDate.Date == date)
                    .Sum(t => t.TransactionAmount);

                result.Add(date, dailySum);
            }

            return result;
        }

        // Get transaction data for last 7 months
        public Dictionary<string, decimal> GetTransactionsForLast7Months(int userId)
        {
            var result = new Dictionary<string, decimal>();
            var allTransactions = _transactionController.GetTransactionsByUserId(userId);

            // Get the last 7 months
            for (int i = 6; i >= 0; i--)
            {
                DateTime date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-i);
                string monthName = date.ToString("MMM yyyy");

                decimal monthlySum = allTransactions
                    .Where(t => t.TransactionsDate.Year == date.Year && t.TransactionsDate.Month == date.Month)
                    .Sum(t => t.TransactionAmount);

                result.Add(monthName, monthlySum);
            }

            return result;
        }

        // Get transaction data by category
        public Dictionary<string, decimal> GetTransactionsByCategory(int userId)
        {
            var result = new Dictionary<string, decimal>();
            var allTransactions = _transactionController.GetTransactionsByUserId(userId);

            // Group by transaction type and sum amounts
            var groupedData = allTransactions
                .GroupBy(t => t.TransactionType.TransactionTypeName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(t => t.TransactionAmount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            foreach (var item in groupedData)
            {
                result.Add(item.CategoryName, item.TotalAmount);
            }

            return result;
        }
    }
}