using Kliker.Models;
using Kliker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kliker.Services
{
    public class GameplayService
    {
        private readonly AppDbContext _context;

        public GameplayService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        public List<Upgrade> GetUpgrades()
        {
            return _context.Upgrades.ToList();
        }

        public List<Achievement> GetAchievements()
        {
            return _context.Achievements.ToList();
        }

        public void UpdatePoints(UpdatePointsModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user == null) return;
            user.Points = model.Points;
            _context.SaveChanges();
        }

        public List<UpgradeViewModel> GetUpgradesReadyToShowOnSite(User user)
        {
            List<Upgrade> allUpgrades = GetUpgrades();
            List<Upgrade> upgradesUnlockedByUser = GetUpgradesUnlockedByUser(user);
            List<UpgradeViewModel> upgradesUnlockedAndNot = MergeAllUpgradesToViewModels(allUpgrades, upgradesUnlockedByUser);
            return upgradesUnlockedAndNot;
        }

        public List<Upgrade> GetUpgradesUnlockedByUser(User user)
        {
            return _context.Upgrades.Where(upgrade => upgrade.PlayersUpgrades.Any(uu => uu.PlayerId == user.Id)).ToList();
        }

        private List<UpgradeViewModel> MergeAllUpgradesToViewModels(List<Upgrade> upgrades, List<Upgrade> unlockedUpgrades)
        {
            List<UpgradeViewModel> upgradeViewModels = new();
            foreach (var upgrade in upgrades)
            {
                if (unlockedUpgrades.Contains<Upgrade>(upgrade)) upgradeViewModels.Add(new UpgradeViewModel(upgrade.Name, upgrade.Description, upgrade.LevelRequired, true));
                else upgradeViewModels.Add(new UpgradeViewModel(upgrade.Name, upgrade.Description, upgrade.LevelRequired, false));
            }
            return upgradeViewModels;
        }

        public int GetRequiredLevelForUpgrade(string upgradeName)
        {
            var upgrade = _context.Upgrades.FirstOrDefault(up => up.Name == upgradeName);
            if (upgrade is null) return -1;
            return upgrade.LevelRequired;
        }

        public void HandleLevelProgression(User user)
        {
            int targetLevel = LevelSystemUtils.CalculateLevelByPoints(user.Points);
            if (user.Lvl < targetLevel) LevelUp(user);
        }

        private void LevelUp(User user)
        {
            Console.Beep();
            user.Lvl++;
            _context.SaveChanges();
        }
    }
}
