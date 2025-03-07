using Kliker.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Kliker.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher<object> _hasher;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            this._hasher = new PasswordHasher<object>();
        }

        public bool IsUsernameAvailable(string username)
        {
            return _appDbContext.Users.Any(u => u.Username == username);
        }

        public bool IsMailAvailable(string mail)
        {
            return _appDbContext.Users.Any(u => u.Email == mail);
        }

        public bool IsUserAvailableByUsernameOrMail(string username, string password)
        {
            if (!IsUsernameAvailable(username) && !IsMailAvailable(username)) return false;
            return true;
        }

        public bool ValidateUserByPassword(string username, string password)
        {
            if(!IsUserAvailableByUsernameOrMail(username, password)) return false;

            User user = null;
            if (IsUsernameAvailable(username)) user = _appDbContext.Users.FirstOrDefault(u => u.Username == username);
            if (IsMailAvailable(username)) user = _appDbContext.Users.FirstOrDefault(u => u.Email == username);
            if (user == null) return false;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success;
        }

        public User CreateUser(RegisterViewModel model)
        {
            var hasher = new PasswordHasher<object>();
            return new User(model.Username, model.Mail, hasher.HashPassword(null, model.Password));
        }

        public void AddUserToDatabase(RegisterViewModel model)
        {
            var newUser = CreateUser(model);
            _appDbContext.Users.Add(newUser);
            _appDbContext.SaveChanges();
        }
    }
}
