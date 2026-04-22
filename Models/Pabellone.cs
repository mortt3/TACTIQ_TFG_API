using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class Pabellone
{
    public int IdPabellon { get; set; }

    public string NombrePab { get; set; } = null!;

    public string? Ciudad { get; set; }

    public virtual ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
