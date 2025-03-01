using Kliker.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Kliker.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher<object> hasher;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            this.hasher = new PasswordHasher<object>();
        }

        public bool IsUsernameAvailable(string username)
        {
            return !_appDbContext.Users.Any(u => u.Username == username);
        }

        public bool IsMailAvailable(string mail)
        {
            return !_appDbContext.Users.Any(u => u.Email == mail);
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
