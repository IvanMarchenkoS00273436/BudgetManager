using BudgetManager.Controllers;
using BudgetManager.Models;
using System.Windows;

namespace BudgetManager.Views.TransactionViews
{
    public partial class AddTransactionWindow : Window
    {
        private TransactionController _transactionController;
        public Transaction? NewTransaction { get; private set; }
        private int _userId;
        public AddTransactionWindow(TransactionController transactionController, int userId)
        {
            InitializeComponent();
            _transactionController = transactionController;
            TypeComboBox.ItemsSource = _transactionController.GetTransactionsTypes();
            _userId = userId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewTransaction = new Transaction
                {
                    TransactionTypeId = ((TransactionType)TypeComboBox.SelectedItem).TransactionTypeId,
                    TransactionAmount = decimal.Parse(AmountTextBox.Text),
                    TransactionsDate = DatePicker.SelectedDate ?? DateTime.Now,
                    UserId = _userId
                };

                if (_transactionController.AddTransaction(NewTransaction))
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
