using Kliker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kliker.Services
{
    public class GameplayService
    {
        private readonly AppDbContext _appDbContext;

        public GameplayService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public List<Upgrade> GetUpgrades()
        {
            return _appDbContext.Upgrades.ToList();
        }

        public List<Achievement> GetAchievements()
        {
            return _appDbContext.Achievements.ToList();
        }

        public void UpdatePoints(UpdatePointsModel model)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user == null) return;
            user.Points = model.Points;
            _appDbContext.SaveChanges();
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
            return _appDbContext.Upgrades.Where(upgrade => upgrade.PlayersUpgrades.Any(uu => uu.PlayerId == user.Id)).ToList();
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
    }
}
