using BudgetManager.Controllers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetManager;

public partial class MainWindow : Window
{
    public UsersController usersController = new UsersController();
    public MainWindow()
    {
        InitializeComponent();
        UserList.ItemsSource = usersController.GetUsers();
    }
}