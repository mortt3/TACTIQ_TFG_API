using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class EstadisticasCampo
{
    public int IdEstadsCampo { get; set; }

    public int IdBase { get; set; }

    public int? TotalLanzamientos { get; set; }

    public int? TotalGoles { get; set; }

    public int? TotalParadas { get; set; }

    public int? TotalPostes { get; set; }

    public int? TotalFuera { get; set; }

    public int? TotalBloqueados { get; set; }

    public int? M7Lanzamientos { get; set; }

    public int? M7Goles { get; set; }

    public int? M7Paradas { get; set; }

    public int? M7Postes { get; set; }

    public int? M7Fuera { get; set; }

    public int? M7Bloqueados { get; set; }

    public int? ContraataqueLanzamientos { get; set; }

    public int? ContraataqueGoles { get; set; }

    public int? ContraataqueParadas { get; set; }

    public int? ContraataquePostes { get; set; }

    public int? ContraataqueFuera { get; set; }

    public int? ContraataqueBloqueados { get; set; }

    public int? M9Lanzamientos { get; set; }

    public int? M9Goles { get; set; }

    public int? M9Paradas { get; set; }

    public int? M9Postes { get; set; }

    public int? M9Fuera { get; set; }

    public int? M9Bloqueados { get; set; }

    public int? ExtremoLanzamientos { get; set; }

    public int? ExtremoGoles { get; set; }

    public int? ExtremoParadas { get; set; }

    public int? ExtremoPostes { get; set; }

    public int? ExtremoFuera { get; set; }

    public int? ExtremoBloqueados { get; set; }

    public int? M6Lanzamientos { get; set; }

    public int? M6Goles { get; set; }

    public int? M6Paradas { get; set; }

    public int? M6Postes { get; set; }

    public int? M6Fuera { get; set; }

    public int? M6Bloqueados { get; set; }

    public int? ValoracionPositivaAsistencia { get; set; }

    public int? ValoracionPositivaRecuperacion { get; set; }

    public int? ValoracionNegativaPerdida { get; set; }

    public int? ValoracionNegativaPasos { get; set; }

    public int? ValoracionNegativaDobles { get; set; }

    public int? ValoracionNegativaFaltaAtaque { get; set; }

    public virtual EstadisticasBase IdBaseNavigation { get; set; } = null!;
}
