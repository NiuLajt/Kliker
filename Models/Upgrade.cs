using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Przechowuje informacje o ulepszeniach nabywanych przez użytkowników. Każdy rekord to pojedyncze ulepszenie reprezentowane przez nazwę i opis.
/// </summary>
public partial class Upgrade
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int LevelRequired { get; set; }

    public Upgrade(string name, string description, decimal price, int levelRequired)
    {
        Name = name;
        Description = description;
        Price = price;
        LevelRequired = levelRequired;
    }
}
