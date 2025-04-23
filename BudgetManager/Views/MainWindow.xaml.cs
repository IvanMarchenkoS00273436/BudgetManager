using BudgetManager.Controllers;
using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using BudgetManager.Views;
using BudgetManager.Views.BudgetViews;
using BudgetManager.Views.TransactionViews;
using System.Collections.ObjectModel;
using System.Windows;

namespace BudgetManager;

public partial class MainWindow : Window
{
    //Controllers
    public UsersController _usersController = new UsersController();
    public TransactionController _transactionController = new TransactionController();
    public BudgetsController _budgetController = new BudgetsController();

    // Data collections
    private List<TransactionType> _transactionTypes;
    private List<Transaction> _transactions;
    private List<Budget> _budgets;
    public User User { get; set;}

    public MainWindow(User currentUser)
    {
        User = currentUser;
        InitializeComponent();

        UpdateUserData();
        InitializeTransactionData();
        InitializeBudgetData();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    //Set the user's name, last name, email and balance
    private void UpdateUserData()
    {
        txbName.Text = User.Name;
        txbLastName.Text = User.LastName;
        txbEmail.Text = User.Email;
        txbBalance.Text = "Current Balance: " + User.Balance.ToString();
    }

    /* Initialize transactions data and budgets for the user */
    /* ======================================== */
    private void InitializeBudgetData()
    {
        //var budgetsList = _budgetController.GetBudgetsByUserId(User.UserId);
        //_budgets = budgetsList;
        //BudgetsListBox.ItemsSource = _budgets;

        // Force a new query from the database
        var budgetsList = _budgetController.GetBudgetsByUserId(User.UserId);
        _budgets = budgetsList;

        // Clear the ItemsSource and set it to null before setting it to the new list
        BudgetsListBox.ItemsSource = null;
        BudgetsListBox.Items.Clear();
        BudgetsListBox.ItemsSource = _budgets;
    }
    private void InitializeTransactionData()
    {
        _transactionTypes = _transactionController.GetTransactionsTypes();
        _transactions = _transactionController.GetTransactionsByUserId(User.UserId);

        TransactionTypeComboBox.ItemsSource = _transactionTypes;
        TransactionsListBox.ItemsSource = _transactions;
    }
    /* ======================================== */


    /* Validation fields in filter */
    /*============================ */
    // Regex for validating user input for field amount
    private void AmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        // Check if the input is a number
        if (!char.IsDigit(e.Text, 0))
        {
            e.Handled = true;
        }
    }

    // Rregex for validation use input for field date
    private void TransactionDatePicker_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        // Check if the input is a number
        if (!char.IsDigit(e.Text, 0) && e.Text != "/")
        {
            e.Handled = true;
        }
    }


    /* Handlers for button/fields/menus */
    /* ========================== */
    private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
    {
        int minAmount = 0;

        if (!string.IsNullOrWhiteSpace(AmountTextBox.Text))
        {
            minAmount = Convert.ToInt32(AmountTextBox.Text);
        }

        _transactions = _transactionController.GetFilteredTransactionsByUserId(
            User.UserId,
            TransactionDatePicker.SelectedDate ?? default,
            minAmount,
            TransactionTypeComboBox.SelectedItem != null ? ((TransactionType)TransactionTypeComboBox.SelectedItem).TransactionTypeId : 0
        );

        // refresh the list
        TransactionsListBox.ItemsSource = null;
        TransactionsListBox.ItemsSource = _transactions;
    }

    private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
    {
        TransactionDatePicker.SelectedDate = null;
        AmountTextBox.Text = string.Empty;
        TransactionTypeComboBox.SelectedItem = null;

        _transactions = _transactionController.GetTransactionsByUserId(User.UserId);
        TransactionsListBox.ItemsSource = null;
        TransactionsListBox.ItemsSource = _transactions;
    }
    private void EditTransactionMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var editTransactionWindow = new EditTransactionWindow(_transactionController, User.UserId, 
            ((Transaction)TransactionsListBox.SelectedItem).TransactionId);

        editTransactionWindow.Owner = this;
        editTransactionWindow.ShowDialog();
        _transactions = _transactionController.GetTransactionsByUserId(User.UserId);
        TransactionsListBox.ItemsSource = null;
        TransactionsListBox.ItemsSource = _transactions;
        InitializeBudgetData();
    }

    private void DeleteTransactionMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var selectedTransaction = (Transaction)TransactionsListBox.SelectedItem;
        if (selectedTransaction != null)
        {
            _transactionController.DeleteTransactionById(selectedTransaction.TransactionId);
            _transactions = _transactionController.GetTransactionsByUserId(User.UserId);
            TransactionsListBox.ItemsSource = null;
            TransactionsListBox.ItemsSource = _transactions;

            InitializeBudgetData();
        }
        else
        {
            var errorWindow = new MessageWindow("Warning!", "Select a transaction to delete");
            errorWindow.Show();
        }
    }

    private void CreateNewTransactionBtn_Click(object sender, RoutedEventArgs e)
    {
        var addTransactionWindow = new AddTransactionWindow(_transactionController, User.UserId);
        addTransactionWindow.Owner = this;
        bool? result = addTransactionWindow.ShowDialog();

        if (result == true)
        {
            // Refresh both transactions and budgets
            _transactions = _transactionController.GetTransactionsByUserId(User.UserId);
            TransactionsListBox.ItemsSource = null;
            TransactionsListBox.ItemsSource = _transactions;

            // Refresh budgets to show updated spent amounts\
            _budgets = null;
            BudgetsListBox.ItemsSource = null;
            InitializeBudgetData();
        }
    }

    private void SaveProfileBtn_Click(object sender, RoutedEventArgs e)
    {
        var oldPassw = psbOldPassword.Password;
        bool isPassCorrect = _usersController.IsPasswordCorrect(User.UserId, oldPassw);
        if (!isPassCorrect)
        {
            var errorWindow = new MessageWindow("Warning!", "Old password is incorrect");
            errorWindow.Show();
            return;
        }

        User updUser = new User()
        {
            UserId = User.UserId,
            Name = txbName.Text,
            LastName = txbLastName.Text,
            Email = txbEmail.Text,
            Balance = User.Balance,
            Password = psbNewPassword.Password
        };

        _usersController.UpdateUser(updUser);
        User = _usersController.GetUsers().FirstOrDefault(u => u.UserId == User.UserId);

        var successWindow = new MessageWindow("Success!", "Profile updated successfully");
        UpdateUserData();
        successWindow.Show();

    }

    private void EditBudgetMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var selectedBudget = (Budget)BudgetsListBox.SelectedItem;
        if (selectedBudget != null) { 
            var editBudgetWindow = new EditBudgetWindow(_budgetController, _transactionController, User.UserId, selectedBudget.BudgetId);
            editBudgetWindow.Owner = this;
            editBudgetWindow.ShowDialog();
            InitializeBudgetData();
        }
        else
        {
            var errorWindow = new MessageWindow("Warning!", "Select a budget to edit");
            errorWindow.Show();
        }
    }

    private void DeleteBudgetMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var selectedBudget = (Budget)BudgetsListBox.SelectedItem;
        if (selectedBudget != null)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the budget for {selectedBudget.TransactionType?.TransactionTypeName}?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_budgetController.DeleteBudget(selectedBudget.BudgetId))
                {
                    // Refresh budgets list
                    InitializeBudgetData();
                }
                else
                {
                    var errorWindow = new MessageWindow("Error", "Could not delete the budget");
                    errorWindow.Show();
                }
            }
        }
        else
        {
            var errorWindow = new MessageWindow("Warning!", "Select a budget to delete");
            errorWindow.Show();
        }
    }

    private void CreateNewBudgetBtn_Click(object sender, RoutedEventArgs e)
    {
        var addBudgetWindow = new AddBudgetView(_budgetController, _transactionController, User.UserId);
        addBudgetWindow.Owner = this;
        addBudgetWindow.ShowDialog();
        InitializeBudgetData();
    }
}