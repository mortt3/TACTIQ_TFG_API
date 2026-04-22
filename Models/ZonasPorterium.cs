using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class ZonasPorterium
{
    public int IdZona { get; set; }

    public int Zona { get; set; }

    public virtual ICollection<PorteroMapaDetalle> PorteroMapaDetalles { get; set; } = new List<PorteroMapaDetalle>();
}
