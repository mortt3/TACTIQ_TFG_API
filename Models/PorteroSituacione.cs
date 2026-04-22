using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class PorteroSituacione
{
    public int IdPorteroSit { get; set; }

    public int IdBase { get; set; }

    public string Situacion { get; set; } = null!;

    public int? GolesRecibidos { get; set; }

    public int? LanzamientosTotales { get; set; }

    public virtual EstadisticasBase IdBaseNavigation { get; set; } = null!;
}
