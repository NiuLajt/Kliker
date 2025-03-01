using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Tabela pomocnicza zawierająca informacje o odblokowaniu ulepszeń przez graczy. Każdy rekord to reprezentacja odblokowania ulepszenia o konkretnym ID przez gracza o konkretnym ID.
/// </summary>
public partial class PlayersUpgrade
{
    public int PlayerId { get; set; }

    public int UpgradeId { get; set; }

    public DateOnly DateOfBeingUpgraded { get; set; }

    public virtual User Player { get; set; }

    public virtual Upgrade Upgrade { get; set; }
}
