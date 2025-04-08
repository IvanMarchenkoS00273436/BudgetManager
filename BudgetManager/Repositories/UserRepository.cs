using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using BudgetManager.Views;

namespace BudgetManager.Repositories
{
    public class UserRepository
    {
        private DbContextData _contextData = new DbContextData();

        // CRUD operations for Users
        public List<User> GetUsers() 
        { 
            return _contextData.Users.ToList(); 
        }
        public bool CreateUser(User user)
        {
            try
            {
                if (
                    string.IsNullOrEmpty(user.Email) ||
                    string.IsNullOrEmpty(user.Name) ||
                    string.IsNullOrEmpty(user.LastName) ||
                    string.IsNullOrEmpty(user.Password))
                {
                    var errorWindow = new MessageWindow("Warning!", "Some of the fields are empty");
                    errorWindow.Show();
                    return false;
                }

                if (_contextData.Users.Any(u => u.Email == user.Email))
                {
                    var errorWindow = new MessageWindow("Warning!", "User with this email already exists");
                    errorWindow.Show();
                    return false;
                }

                User newUser = new User()
                {
                    Email = user.Email,
                    Name = user.Name,
                    LastName = user.LastName,
                    Password = getHashFromPassword(user.Password),
                    Balance = 0
                };

                _contextData.Users.Add(newUser);
                _contextData.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            { 
                var errorMessage = new MessageWindow("Error!", ex.Message);
                errorMessage.Show();
                return false;
            }
        }
        public bool UpdateUser(User user)
        {
            try
            {
                if (
                    string.IsNullOrEmpty(user.Email) ||
                    string.IsNullOrEmpty(user.Name) ||
                    string.IsNullOrEmpty(user.LastName) ||
                    string.IsNullOrEmpty(user.Password))
                {
                    var errorWindow = new MessageWindow("Warning!", "Some of the fields are empty");
                    errorWindow.Show();
                    return false;
                }
                User userToUpdate = _contextData.Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (userToUpdate == null)
                {
                    var errorWindow = new MessageWindow("Warning!", "User does not exist");
                    errorWindow.Show();
                    return false;
                }
                userToUpdate.Email = user.Email;
                userToUpdate.Name = user.Name;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Password = getHashFromPassword(user.Password);
                _contextData.SaveChanges();
                return true;
            }
            catch (Exception ex) { throw new Exception("Problem with updating user"); }
        }
        public bool DeleteUser(int userId)
        {
            try
            {
                User userToDelete = _contextData.Users.FirstOrDefault(u => u.UserId == userId);
                if (userToDelete == null)
                {
                    var errorWindow = new MessageWindow("Warning!", "User does not exist");
                    errorWindow.Show();
                    return false;
                }
                _contextData.Users.Remove(userToDelete);
                _contextData.SaveChanges();
                return true;
            }
            catch (Exception ex) { throw new Exception("Problem with deleting user"); }
        }


        // Login and password hashing
        public User? LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
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
                var infoWindow = new MessageWindow("Passwords check!",
                    $"{BCrypt.Net.BCrypt.Verify(password, user.Password)}" +
                    $": {BCrypt.Net.BCrypt.HashPassword(password)} = {user.Password}");
                infoWindow.Show();
                return null;
            }
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
        public bool IsPasswordCorrect(int userId,string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                var errorWindow = new MessageWindow("Warning!", "Password is empty");
                errorWindow.Show();
                return false;
            }

            var user = _contextData.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                var errorWindow = new MessageWindow("Warning!", "User not found");
                errorWindow.Show();
                return false;
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isPasswordCorrect;
        }
    }
}
