namespace Kliker.Models
{
    public class UpgradeStatusModel(string nameOfUpgrade)
    {
        public string NameOfUpgrade { get; set; } = nameOfUpgrade;
    }
}
