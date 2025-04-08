using BudgetManager.Models;
using BudgetManager.Repositories;

namespace BudgetManager.Controllers
{
    public class UsersController
    {
        private UserRepository _userRepository = new UserRepository();

        // CRUD operations for Users
        public List<User> GetUsers() 
        { 
            return _userRepository.GetUsers();
        }
        public bool CreateUser(User user)
        {
            return _userRepository.CreateUser(user);
        }
        public User? LoginUser(string email, string password) 
        {
            return _userRepository.LoginUser(email, password);
        }
        public void UpdateUser(User user) 
        {
            _userRepository.UpdateUser(user);
        }


        public string getHashFromPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    throw new Exception("Password is empty");
                }
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex) { throw new Exception("Problem with converting password to hash"); }
        }
        public bool IsPasswordCorrect(int userId, string password)
        {
            return _userRepository.IsPasswordCorrect(userId, password);
        }
    }
}
