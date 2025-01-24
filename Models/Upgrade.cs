using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Przechowuje informacje o ulepszeniach nabywanych przez użytkowników. Każdy rekord to pojedyncze ulepszenie reprezentowane przez nazwę i opis.
/// </summary>
public partial class Upgrade(string name, string description, decimal price, int levelRequired)
{
    public int Id { get; set; }

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public int LevelRequired { get; set; } = levelRequired;

    public virtual ICollection<PlayersUpgrade> PlayersUpgrades { get; set; } = [];
}