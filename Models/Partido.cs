using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class Partido
{
    public int IdPartido { get; set; }

    public int IdEquipoLocal { get; set; }

    public int IdEquipoVisitante { get; set; }

    public int IdTemporada { get; set; }

    public int? IdPabellon { get; set; }

    public int? Jornada { get; set; }

    public DateOnly? Fecha { get; set; }

    public TimeOnly? Hora { get; set; }

    public int? GolesLocal { get; set; }

    public int? GolesVisitante { get; set; }

    public string? Condicion { get; set; }

    public virtual ICollection<EstadisticasBase> EstadisticasBases { get; set; } = new List<EstadisticasBase>();

    public virtual Equipo IdEquipoLocalNavigation { get; set; } = null!;

    public virtual Equipo IdEquipoVisitanteNavigation { get; set; } = null!;

    public virtual Pabellone? IdPabellonNavigation { get; set; }

    public virtual Temporada IdTemporadaNavigation { get; set; } = null!;
}
