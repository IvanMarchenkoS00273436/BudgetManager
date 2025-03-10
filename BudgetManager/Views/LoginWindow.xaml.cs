using BudgetManager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BudgetManager.Models;

namespace BudgetManager.Views
{
    public partial class LoginWindow : Window
    {
        private UsersController usersController = new UsersController();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //string email = txtEmail.Text;
            //string password = txtPassword.Password;

            //User checkUser = usersController.LoginUser(email, password);

            //if (checkUser != null) {
            //    var MainWindow = new MainWindow(checkUser);
            //    this.Close();
            //    MainWindow.Show();
            //}

            var MainWindow = new MainWindow(null);
               this.Close();
                MainWindow.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of RegisterWindow
            var registerWindow = new RegisterWindow();

            // Close the current window
            this.Close();

            // Show the register window
            registerWindow.Show();
        }
    }
}
