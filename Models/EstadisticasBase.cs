using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class EstadisticasBase
{
    public int IdBase { get; set; }

    public int IdJugador { get; set; }

    public int IdPartido { get; set; }

    public int? IdSituacion { get; set; }

    public string? TiempoDef { get; set; }

    public string? TiempoTot { get; set; }

    public decimal? ValoracionGlobal { get; set; }

    public int? SancionAmarilla { get; set; }

    public int? Sancion2mins1 { get; set; }

    public int? Sancion2mins2 { get; set; }

    public int? SancionRoja { get; set; }

    public int? SancionAzul { get; set; }

    public virtual EstadisticasCampo? EstadisticasCampo { get; set; }

    public virtual EstadisticasPortero? EstadisticasPortero { get; set; }

    public virtual Jugadore IdJugadorNavigation { get; set; } = null!;

    public virtual Partido IdPartidoNavigation { get; set; } = null!;

    public virtual SituacionJuego? IdSituacionNavigation { get; set; }

    public virtual ICollection<PorteroMapaDetalle> PorteroMapaDetalles { get; set; } = new List<PorteroMapaDetalle>();

    public virtual ICollection<PorteroSituacione> PorteroSituaciones { get; set; } = new List<PorteroSituacione>();
}
