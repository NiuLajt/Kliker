using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Tabela pośrednia pomiędzy tabelą użytkowników a tabelą osiągnięć. Przechowuje ona informacje o zdobyciu przez konkretnego użytkownika konkretnego osiągnięcia wraz z datą tego wydarzenia.
/// </summary>
public partial class PlayersAchievement
{
    public int PlayerId { get; set; }

    public int AchievementId { get; set; }

    public DateOnly DateIfBeingAchieved { get; set; }

    public virtual Achievement Achievement { get; set; }

    public virtual User Player { get; set; }
}
