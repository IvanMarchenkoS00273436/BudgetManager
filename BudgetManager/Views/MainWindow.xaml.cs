using BudgetManager.Controllers;
using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using BudgetManager.Views;
using System.Windows;

namespace BudgetManager;

public partial class MainWindow : Window
{
    public UsersController _usersController = new UsersController();
    public User User { get; set;}

    public MainWindow(User currentUser)
    {
        User = currentUser;
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //Set the user's name, last name, email and balance
        UpdateUserData();
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
}