using System.Windows;

namespace BudgetManager.Views
{
    public partial class MessageWindow : Window
    {
        public string TitleMessage { get; set; }
        public string TextMessage { get; set; }

        public MessageWindow(string title, string message)
        {
            InitializeComponent();

            //Data binding
            TitleMessage = title;
            TextMessage = message;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
