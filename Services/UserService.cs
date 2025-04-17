using Kliker.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kliker.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<object> _hasher;

        public UserService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            this._hasher = new PasswordHasher<object>();
        }

        public bool IsUsernameAvailable(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public bool IsMailAvailable(string mail)
        {
            return _context.Users.Any(u => u.Email == mail);
        }

        public bool IsUserAvailableByUsernameOrMail(string username)
        {
            if (!IsUsernameAvailable(username) && !IsMailAvailable(username)) return false;
            return true;
        }

        public User GetUserFromDatabase(string username)
        {
            if (IsUserAvailableByUsernameOrMail(username)) return _context.Users.FirstOrDefault(u => u.Username == username || u.Email == username);
            else return null;
        }

        public bool ValidateUserByPassword(string username, string password)
        {
            if(!IsUserAvailableByUsernameOrMail(username)) return false;

            User user = null;
            if (IsUsernameAvailable(username)) user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (IsMailAvailable(username)) user = _context.Users.FirstOrDefault(u => u.Email == username);
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
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public bool IsUpgradeAlreadyUnlockedByUser(User user, string upgradeName)
        {
            var upgradeByName = _context.Upgrades.FirstOrDefault(up => up.Name == upgradeName);
            return _context.PlayersUpgrades.FirstOrDefault(row => row.UpgradeId == upgradeByName.Id && row.PlayerId == user.Id) is not null;
        }
    }
}