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
            User regUser = new User() 
            {
                Email = txtEmail.Text,
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Password = txtPassword.Password
            };

            if(usersController.CreateUser(regUser))
            {
                new MessageWindow("Success!", "Registration succesfull!").Show();
                this.Close();
                new LoginWindow().Show();
            }
        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
