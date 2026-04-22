using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class VTotalesCampo
{
    public int? IdJugador { get; set; }

    public string? NombreJugador { get; set; }

    public int? Dorsal { get; set; }

    public string? Posicion { get; set; }

    public long? PartidosJugados { get; set; }

    public long? MinutosTotales { get; set; }

    public decimal? ValoracionTotal { get; set; }

    public decimal? ValoracionMedia { get; set; }

    public long? TotalLanzamientos { get; set; }

    public long? TotalGoles { get; set; }

    public decimal? PctEfectividad { get; set; }

    public long? M9Lanz { get; set; }

    public long? M9Goles { get; set; }

    public long? ExtLanz { get; set; }

    public long? ExtGoles { get; set; }

    public long? M6Lanz { get; set; }

    public long? M6Goles { get; set; }

    public long? M7Lanz { get; set; }

    public long? M7Goles { get; set; }

    public long? Asistencias { get; set; }

    public long? Recuperaciones { get; set; }

    public long? Perdidas { get; set; }

    public long? Pasos { get; set; }

    public long? Dobles { get; set; }

    public long? FaltasAtaque { get; set; }

    public long? TarjetasAmarillas { get; set; }

    public long? Exclusiones2min { get; set; }

    public long? TarjetasRojas { get; set; }

    public long? TarjetasAzules { get; set; }
}
