using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class JugadoresRivale
{
    public int IdJugadorRival { get; set; }

    public int? IdEquipo { get; set; }

    public string? Nombre { get; set; }

    public int? Dorsal { get; set; }

    public string? Posicion { get; set; }

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual ICollection<PorteroMapaDetalle> PorteroMapaDetalles { get; set; } = new List<PorteroMapaDetalle>();
}
