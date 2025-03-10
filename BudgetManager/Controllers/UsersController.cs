using BCrypt.Net;
using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using BudgetManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Controllers
{
    public class UsersController
    {
        private DbContextData _contextData;

        public UsersController() 
        {
            _contextData = new DbContextData();
        }

        public List<User> GetUsers() { return _contextData.Users.ToList(); }

        public bool CreateUser(string email, string name, string lastName, string password)
        {
            try
            {
                if(
                    string.IsNullOrEmpty(email) || 
                    string.IsNullOrEmpty(name) || 
                    string.IsNullOrEmpty(lastName) || 
                    string.IsNullOrEmpty(password))
                {
                    var errorWindow = new MessageWindow("Warning!", "Some of the fields are empty");
                    errorWindow.Show();
                    return false;
                }

                if(_contextData.Users.Any(u => u.Email == email))
                {
                    var errorWindow = new MessageWindow("Warning!", "User with this email already exists");
                    errorWindow.Show();
                    return false;
                }

                User newUser = new User()
                {
                    Email = email,
                    Name = name,
                    LastName = lastName,
                    Password = getHashFromPassword(password),
                    Balance = 0
                };

                _contextData.Users.Add(newUser);
                _contextData.SaveChanges();
                return true;
            }
            catch (Exception ex) { throw new Exception("Problem with creating user"); }
        }

        public User? LoginUser(string email, string password) 
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                var errorWindow = new MessageWindow("Warning!", "Some of the fields are empty");
                errorWindow.Show();
                return null;
            }

            User user = _contextData.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                var errorWindow = new MessageWindow("Warning!", "User with this email does not exist");
                errorWindow.Show();
                return null;
            } 
            else if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            } 
            else
            {
                var errorWindow = new MessageWindow("Warning!", "Password or Email is incorrect");
                errorWindow.Show();
                return null;
            }
        }
        public void LogoutUser() { }
        public void UpdateUser(User user) { }

        private string getHashFromPassword(string password)
        {
            try { 
                if(string.IsNullOrEmpty(password))
                {
                    throw new Exception("Password is empty");
                }
                return BCrypt.Net.BCrypt.HashPassword(password); 
            }
            catch (Exception ex) { throw new Exception("Problem with converting password to hash"); }
        }
    }
}
