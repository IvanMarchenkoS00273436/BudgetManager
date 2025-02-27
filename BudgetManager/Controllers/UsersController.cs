using BudgetManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Controllers
{
    public class UsersController
    {
        private List<User> Users = new List<User>();

        public UsersController() 
        {
            for (int i = 0; i < 10; i++)
            {
                User user = new User();
                user.UserId = i;
                user.Name = "Name" + i;
                user.LastName = "LastName" + i;
                user.Email = "email" + i + "@gmail.com";
                user.Password = "password" + i;
                user.Balance = 1000;
                Users.Add(user);
            }
        }

        public List<User> GetUsers()
        {
            return Users;
        }
    }
}
