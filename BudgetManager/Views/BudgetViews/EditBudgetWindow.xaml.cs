using BudgetManager.Controllers;
using BudgetManager.Models;
using System.Windows;

namespace BudgetManager.Views.BudgetViews
{
    public partial class EditBudgetWindow : Window
    {
        private readonly BudgetsController _budgetController;
        private readonly TransactionController _transactionController;
        private readonly int _budgetId;
        private readonly int _userId;

        public EditBudgetWindow(BudgetsController budgetController, 
            TransactionController transactionController, int userId, int budgetId)
        {
            InitializeComponent();
            _budgetController = budgetController;
            _transactionController = transactionController;
            _userId = userId;
            _budgetId = budgetId;
            LoadBudgetData();
            LoadTransactionTypes();
        }

        private void LoadBudgetData()
        {
            var budget = _budgetController.GetBudgetById(_budgetId);
            if (budget != null)
            {
                BudgetAmountTextBox.Text = budget.BudgetAmount.ToString();
                StartDatePicker.SelectedDate = budget.StartDate;
                EndDatePicker.SelectedDate = budget.EndDate;
                CategoryComboBox.SelectedValue = budget.TransactionTypeId;
            }
        }

        private void LoadTransactionTypes()
        {
            CategoryComboBox.ItemsSource = _transactionController.GetTransactionsTypes();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CategoryComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a category", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(BudgetAmountTextBox.Text) ||
                    !decimal.TryParse(BudgetAmountTextBox.Text, out decimal budgetAmount) ||
                    budgetAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid budget amount", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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

                // Get current budget to preserve SpentAmount
                var currentBudget = _budgetController.GetBudgetById(_budgetId);
                if (currentBudget == null)
                {
                    MessageBox.Show("Could not retrieve current budget data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var updatedBudget = new Budget
                {
                    BudgetId = _budgetId,
                    UserId = _userId,
                    TransactionTypeId = (int)CategoryComboBox.SelectedValue,
                    BudgetAmount = budgetAmount,
                    StartDate = StartDatePicker.SelectedDate.Value,
                    EndDate = EndDatePicker.SelectedDate.Value,
                    SpentAmount = currentBudget.SpentAmount // Preserve the spent amount
                };

                if (_budgetController.UpdateBudget(updatedBudget))
                {
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
