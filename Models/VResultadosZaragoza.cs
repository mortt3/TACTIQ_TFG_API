using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class VResultadosZaragoza
{
    public int? IdPartido { get; set; }

    public int? Jornada { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Condicion { get; set; }

    public string? EquipoLocal { get; set; }

    public int? GolesLocal { get; set; }

    public int? GolesVisitante { get; set; }

    public string? EquipoVisitante { get; set; }

    public string? Resultado { get; set; }

    public int? GolesZaragoza { get; set; }

    public int? GolesRival { get; set; }
}
