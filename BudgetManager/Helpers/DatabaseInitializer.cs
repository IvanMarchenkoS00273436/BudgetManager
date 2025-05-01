using BudgetManager.Controllers;
using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManager.Helpers
{
    // Class to initialize a sql database with some random data
    public static class DatabaseInitializer
    {
        public static void InitializeSampleData()
        {
            try
            {
                using (var context = new DbContextData())
                {
                    // Check if database is already initialized
                    if (context.Users.Any())
                    {
                        return; // Data exists, no need to initialize
                    }

                    // Create controllers
                    var usersController = new UsersController();
                    var transactionController = new TransactionController();
                    var budgetsController = new BudgetsController();

                    // 1. Create a user
                    var user = new User
                    {
                        Name = "Ivan",
                        LastName = "Marchenko",
                        Email = "S00273436@atu.ie",
                        Password = "Ivan123",
                        Balance = 5000m
                    };

                    usersController.CreateUser(user);

                    // Get the created user with ID
                    var createdUser = context.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");
                    if (createdUser == null)
                    {
                        throw new Exception("Failed to create user");
                    }

                    // 2. Get transaction types
                    var transactionTypes = transactionController.GetTransactionsTypes();
                    if (!transactionTypes.Any())
                    {
                        // Create transaction types if none exist
                        CreateTransactionTypes(context);
                        transactionTypes = transactionController.GetTransactionsTypes();
                    }

                    // 3. Create random transactions
                    CreateSampleTransactions(transactionController, createdUser.UserId, transactionTypes, 15);

                    // 4. Create budgets
                    CreateSampleBudgets(budgetsController, createdUser.UserId, transactionTypes, 7);

                    Console.WriteLine("Database initialized with sample data");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        private static void CreateTransactionTypes(DbContextData context)
        {
            var types = new List<TransactionType>
            {
                new TransactionType { TransactionTypeName = "Groceries", PathToPhoto = "/Assets/Icons/TransactionTypes/Groceries.png" },
                new TransactionType { TransactionTypeName = "Utilities", PathToPhoto = "/Assets/Icons/TransactionTypes/Utilities.png" },
                new TransactionType { TransactionTypeName = "Rent", PathToPhoto = "/Assets/Icons/TransactionTypes/Rent.png" },
                new TransactionType { TransactionTypeName = "Transportation", PathToPhoto = "/Assets/Icons/TransactionTypes/Transport.png" },
                new TransactionType { TransactionTypeName = "Entertainment", PathToPhoto = "/Assets/Icons/TransactionTypes/Entertainment.png" },
                new TransactionType { TransactionTypeName = "Dining Out", PathToPhoto = "/Assets/Icons/TransactionTypes/DiningOut.png" },
                new TransactionType { TransactionTypeName = "Healthcare", PathToPhoto = "/Assets/Icons/TransactionTypes/Healthcare.png" },
                new TransactionType { TransactionTypeName = "Shopping", PathToPhoto = "/Assets/Icons/TransactionTypes/Shopping.png" },
                new TransactionType { TransactionTypeName = "Education", PathToPhoto = "/Assets/Icons/TransactionTypes/Education.png" },
                new TransactionType { TransactionTypeName = "Other", PathToPhoto = "/Assets/Icons/TransactionTypes/Other.png" }
            };

            context.TransactionTypes.AddRange(types);
            context.SaveChanges();
        }
        private static void CreateSampleTransactions(TransactionController transactionController, int userId, List<TransactionType> types, int count)
        {
            var random = new Random();

            // Create transactions for the last 3 months
            var startDate = DateTime.Now.AddMonths(-3);

            for (int i = 0; i < count; i++)
            {
                // Random transaction type
                var typeIndex = random.Next(0, types.Count);
                var type = types[typeIndex];

                // Random date within the last 3 months
                var daysToAdd = random.Next(0, (DateTime.Now - startDate).Days);
                var transactionDate = startDate.AddDays(daysToAdd);

                // Random amount between 10 and 500
                var amount = random.Next(10, 501);

                var transaction = new Transaction
                {
                    UserId = userId,
                    TransactionTypeId = type.TransactionTypeId,
                    TransactionAmount = amount,
                    TransactionsDate = transactionDate
                };

                transactionController.AddTransaction(transaction);
            }
        }
        private static void CreateSampleBudgets(BudgetsController budgetsController, int userId, List<TransactionType> types, int count)
        {
            var random = new Random();
            var currentDate = DateTime.Now;

            // Use a HashSet to track which transaction types have been used
            var usedTypeIds = new HashSet<int>();

            for (int i = 0; i < count && i < types.Count; i++)
            {
                // Select a transaction type that hasn't been used yet
                int typeIndex;
                do
                {
                    typeIndex = random.Next(0, types.Count);
                } while (usedTypeIds.Contains(types[typeIndex].TransactionTypeId));

                var type = types[typeIndex];
                usedTypeIds.Add(type.TransactionTypeId);

                // Create start and end dates
                var startDate = currentDate.AddDays(-random.Next(1, 10));
                var endDate = currentDate.AddDays(random.Next(20, 60));

                // Budget amount between 100 and 1000
                var budgetAmount = random.Next(100, 1001);

                var budget = new Budget
                {
                    UserId = userId,
                    TransactionTypeId = type.TransactionTypeId,
                    StartDate = startDate,
                    EndDate = endDate,
                    BudgetAmount = budgetAmount,
                    SpentAmount = 0
                };

                budgetsController.AddBudget(budget);
            }
        }
    }
}