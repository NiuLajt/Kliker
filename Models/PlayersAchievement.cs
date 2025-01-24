using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Tabela pośrednia pomiędzy tabelą użytkowników a tabelą osiągnięć. Przechowuje ona informacje o zdobyciu przez konkretnego użytkownika konkretnego osiągnięcia wraz z datą tego wydarzenia.
/// </summary>
public partial class PlayersAchievement(User player, Achievement achievement, DateOnly dateIfBeingAchieved)
{
    public int PlayerId { get; set; } = player.Id;

    public int AchievementId { get; set; }

    public DateOnly DateIfBeingAchieved { get; set; } = dateIfBeingAchieved;

    public virtual Achievement Achievement { get; set; } = achievement;

    public virtual User Player { get; set; } = player;
}
