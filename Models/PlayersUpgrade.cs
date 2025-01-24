using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Tabela pomocnicza zawierająca informacje o odblokowaniu ulepszeń przez graczy. Każdy rekord to reprezentacja odblokowania ulepszenia o konkretnym ID przez gracza o konkretnym ID.
/// </summary>
public partial class PlayersUpgrade(User player, Upgrade upgrade, DateOnly dateOfBeingUpgraded)
{
    public int PlayerId { get; set; } = player.Id;

    public int UpgradeId { get; set; } = upgrade.Id;

    public DateOnly DateOfBeingUpgraded { get; set; } = dateOfBeingUpgraded;

    public virtual User Player { get; set; } = player;

    public virtual Upgrade Upgrade { get; set; } = upgrade;
}
