using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class SituacionJuego
{
    public int IdSituacion { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<EstadisticasBase> EstadisticasBases { get; set; } = new List<EstadisticasBase>();
}
