namespace Kliker.Models
{
    public class UpgradeViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int LevelRequired { get; set; }

        public bool IsAlreadyUnlocked { get; set; }

        public UpgradeViewModel(string name, string description, int levelRequired, bool isAlredyUnlocked)
        {
            Name = name;
            Description = description;
            LevelRequired = levelRequired;
            IsAlreadyUnlocked = isAlredyUnlocked;
        }
    }
}