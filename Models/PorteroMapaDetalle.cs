using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class PorteroMapaDetalle
{
    public int IdMapa { get; set; }

    public int IdBase { get; set; }

    public int IdZona { get; set; }

    public int? IdJugadorRival { get; set; }

    public string ResultadoJugada { get; set; } = null!;

    public virtual EstadisticasBase IdBaseNavigation { get; set; } = null!;

    public virtual JugadoresRivale? IdJugadorRivalNavigation { get; set; }

    public virtual ZonasPorterium IdZonaNavigation { get; set; } = null!;
}
