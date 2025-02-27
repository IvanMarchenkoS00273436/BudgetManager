using BudgetManager.Controllers;
using BudgetManager.Models;
using System.Windows;
using BCrypt.Net;


namespace BudgetManager.Views
{
    public partial class RegisterWindow : Window
    {
        UsersController usersController = new UsersController();
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                Email = txtEmail.Text,
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Password),

            };
            if (usersController.GetUsers().Any(u => u.Email == newUser.Email))
            {
                var msgWindow = new MessageWindow("Error", "User with this email already exists!");
                msgWindow.ShowDialog();
                return;
            }

            usersController.GetUsers().Add(newUser);
            MessageBox.Show("Registration successful!");
            this.Close();
        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
