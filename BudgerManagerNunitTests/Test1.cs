using BudgetManager.Controllers;
using BudgetManager.Models;

namespace BudgerManagerNunitTests
{
    [TestClass]
    public sealed class Test1
    {
        private UsersController _usersController;
        private TransactionController _transactionController;
        private BudgetsController _budgetsController;

        [TestInitialize]
        public void Setup()
        {
            _usersController = new UsersController();
            _transactionController = new TransactionController();
            _budgetsController = new BudgetsController();
        }

        // Test user password hashing
        [TestMethod]
        public void TestPasswordHashing()
        {
            // Arrange
            string password = "test123";

            // Act
            string hashedPassword = _usersController.getHashFromPassword(password);

            // Assert
            Assert.IsNotNull(hashedPassword);
            Assert.AreNotEqual(password, hashedPassword);
        }

        // Test budget validation
        [TestMethod]
        public void TestBudgetValidation()
        {
            // Arrange
            var invalidBudget = new Budget
            {
                UserId = 1,
                TransactionTypeId = 1,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now,  // End date before start date
                BudgetAmount = 100
            };

            // Act & Assert
            Assert.ThrowsException<Exception>(() => _budgetsController.AddBudget(invalidBudget));
        }

        // Test transaction type retrieval
        [TestMethod]
        public void TestGetTransactionTypes()
        {
            // Act
            var types = _transactionController.GetTransactionsTypes();

            // Assert
            Assert.IsNotNull(types);
            Assert.IsTrue(types.Count > 0);
        }
    }
}
