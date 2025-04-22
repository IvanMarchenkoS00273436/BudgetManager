using BudgetManager.Controllers;
using BudgetManager.Models;
using System.Windows;

namespace BudgetManager.Views.TransactionViews
{
    public partial class EditTransactionWindow : Window
    {
        private readonly TransactionController _controller;
        private readonly int _transactionId;
        private readonly int _userId;
        public EditTransactionWindow(TransactionController controller, int userId, int transactionId)
        {
            InitializeComponent();
            _controller = controller;
            _userId = userId;
            _transactionId = transactionId;
            LoadTransactionData();
            LoadTransactionTypes();
        }

        private void LoadTransactionData()
        {
            var transaction = _controller.GetTransactionById(_transactionId);
            if (transaction != null)
            {
                AmountTextBox.Text = transaction.TransactionAmount.ToString();
                DatePicker.SelectedDate = transaction.TransactionsDate;
                TypeComboBox.SelectedValue = transaction.TransactionTypeId;
            }
        }

        private void LoadTransactionTypes()
        {
            TypeComboBox.ItemsSource = _controller.GetTransactionsTypes();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updatedTransaction = new Transaction
                {
                    TransactionId = _transactionId,
                    UserId = _userId,
                    TransactionTypeId = (int)TypeComboBox.SelectedValue,
                    TransactionAmount = decimal.Parse(AmountTextBox.Text),
                    TransactionsDate = DatePicker.SelectedDate ?? DateTime.Now
                };

                if (_controller.EditTransaction(updatedTransaction))
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
