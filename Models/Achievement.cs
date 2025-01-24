using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Przechowuje informacje o osiągnięciach. Każdy rekord to jedno dostępne do zdobycia osiągnięcie reprezentowane poprzez nazwę oraz opis.
/// </summary>
public partial class Achievement(string name, string description)
{
    public int Id { get; set; }

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public virtual ICollection<PlayersAchievement> PlayersAchievements { get; set; } = [];
}