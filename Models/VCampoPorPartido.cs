using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class VCampoPorPartido
{
    public int? IdJugador { get; set; }

    public string? NombreJugador { get; set; }

    public int? Dorsal { get; set; }

    public string? Posicion { get; set; }

    public int? IdPartido { get; set; }

    public int? Jornada { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? EquipoLocal { get; set; }

    public string? EquipoVisitante { get; set; }

    public int? GolesLocal { get; set; }

    public int? GolesVisitante { get; set; }

    public string? Condicion { get; set; }

    public string? TiempoTot { get; set; }

    public decimal? ValoracionGlobal { get; set; }

    public int? TotalLanzamientos { get; set; }

    public int? TotalGoles { get; set; }

    public int? M7Goles { get; set; }

    public int? M9Goles { get; set; }

    public int? ExtremoGoles { get; set; }

    public int? M6Goles { get; set; }

    public int? Asistencias { get; set; }

    public int? Recuperaciones { get; set; }

    public int? Perdidas { get; set; }

    public int? Exclusiones { get; set; }
}
