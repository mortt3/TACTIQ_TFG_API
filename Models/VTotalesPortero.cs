using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class VTotalesPortero
{
    public int? IdJugador { get; set; }

    public string? NombreJugador { get; set; }

    public int? Dorsal { get; set; }

    public long? PartidosJugados { get; set; }

    public long? MinutosTotales { get; set; }

    public decimal? ValoracionTotal { get; set; }

    public long? LanzamientosRecibidos { get; set; }

    public long? GolesRecibidos { get; set; }

    public long? Paradas { get; set; }

    public decimal? PctParadas { get; set; }

    public long? GolesPosicional { get; set; }

    public long? LanzPosicional { get; set; }

    public long? GolesContra { get; set; }

    public long? LanzContra { get; set; }

    public long? Goles7m { get; set; }

    public long? Lanz7m { get; set; }

    public long? TarjetasAmarillas { get; set; }

    public long? Exclusiones2min { get; set; }
}
