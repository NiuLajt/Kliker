using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Przechowuje informacje o osiągnięciach. Każdy rekord to jedno dostępne do zdobycia osiągnięcie reprezentowane poprzez nazwę oraz opis.
/// </summary>
public partial class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<PlayersAchievement> PlayersAchievments { get; set; } = new List<PlayersAchievement>();
}
