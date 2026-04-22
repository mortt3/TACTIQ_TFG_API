using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class Temporada
{
    public int IdTemporada { get; set; }

    public DateOnly Anyo { get; set; }

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
