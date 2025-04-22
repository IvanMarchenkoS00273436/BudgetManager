using BudgetManager.Controllers;
using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using BudgetManager.Views;
using BudgetManager.Views.TransactionViews;
using System.Windows;

namespace BudgetManager;

public partial class MainWindow : Window
{
    //private readonly DbContextData _dbContext = new DbContextData();
    public UsersController _usersController = new UsersController();
    public TransactionController _transactionController = new TransactionController();
    private List<TransactionType> _transactionTypes;
    private List<Transaction> _transactions;
    public User User { get; set;}

    public MainWindow(User currentUser)
    {
        User = currentUser;
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateUserData();
        InitializeTransactionData();
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

    //Set the user's name, last name, email and balance
    private void UpdateUserData()
    {
        txbName.Text = User.Name;
        txbLastName.Text = User.LastName;
        txbEmail.Text = User.Email;
        txbBalance.Text = "Current Balance: " + User.Balance.ToString();
    }

    public void InitializeTransactionData()
    {
        _transactionTypes = _transactionController.GetTransactionsTypes();
        _transactions = _transactionController.GetTransactionsByUserId(User.UserId);

        TransactionTypeComboBox.ItemsSource = _transactionTypes;
        TransactionsListBox.ItemsSource = _transactions;
    }

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

    // Write a regex for validation use input for field date
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
        addTransactionWindow.ShowDialog();
        _transactions = _transactionController.GetTransactionsByUserId(User.UserId);
        TransactionsListBox.ItemsSource = null;
        TransactionsListBox.ItemsSource = _transactions;
    }
}