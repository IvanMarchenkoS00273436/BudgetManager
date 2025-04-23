using BudgetManager.Controllers;
using BudgetManager.Models;
using BudgetManager.Views;
using System;
using System.Linq;
using System.Windows;

namespace BudgetManager.Views.BudgetViews
{
    public partial class AddBudgetView : Window
    {
        private readonly BudgetsController _budgetController;
        private readonly TransactionController _transactionController;
        private readonly int _userId;

        public AddBudgetView(BudgetsController budgetController, 
            TransactionController transactionController, int userId)
        {
            InitializeComponent();
            _budgetController = budgetController;
            _transactionController = transactionController;
            _userId = userId;

            // Load transaction types into combo box
            LoadTransactionTypes();

            // Set default dates
            StartDatePicker.SelectedDate = DateTime.Now;
            EndDatePicker.SelectedDate = DateTime.Now.AddMonths(1);
        }

        private void LoadTransactionTypes()
        {
            CategoryComboBox.ItemsSource = _transactionController.GetTransactionsTypes();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate category selection
                if (CategoryComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a category", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int selectedCategoryId = ((TransactionType)CategoryComboBox.SelectedItem).TransactionTypeId;

                // Check if budget for this category already exists
                var existingBudgets = _budgetController.GetBudgetsByUserId(_userId);
                if (existingBudgets.Any(b => b.TransactionTypeId == selectedCategoryId))
                {
                    MessageBox.Show(
                        "A budget for this category already exists. Please edit the existing budget instead.",
                        "Duplicate Budget",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Validate budget amount
                if (string.IsNullOrWhiteSpace(BudgetAmountTextBox.Text) ||
                    !decimal.TryParse(BudgetAmountTextBox.Text, out decimal budgetAmount) ||
                    budgetAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid budget amount", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate dates
                if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please select both start and end dates", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
                {
                    MessageBox.Show("Start date cannot be after end date", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new budget
                var newBudget = new Budget
                {
                    UserId = _userId,
                    TransactionTypeId = selectedCategoryId,
                    BudgetAmount = budgetAmount,
                    StartDate = StartDatePicker.SelectedDate.Value,
                    EndDate = EndDatePicker.SelectedDate.Value,
                    SpentAmount = 0 // Initialize spent amount to 0
                };

                // Add budget
                if (_budgetController.AddBudget(newBudget))
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    var errorWindow = new MessageWindow("Error", "Failed to add budget");
                    errorWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}