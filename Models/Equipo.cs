using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class Equipo
{
    public int IdEquipo { get; set; }

    public string NombreEquipo { get; set; } = null!;

    public string? ImagenLogo { get; set; }

    public string? Ciudad { get; set; }

    public int? IdPabellon { get; set; }

    public virtual Pabellone? IdPabellonNavigation { get; set; }

    public virtual ICollection<Jugadore> Jugadores { get; set; } = new List<Jugadore>();

    public virtual ICollection<JugadoresRivale> JugadoresRivales { get; set; } = new List<JugadoresRivale>();

    public virtual ICollection<Partido> PartidoIdEquipoLocalNavigations { get; set; } = new List<Partido>();

    public virtual ICollection<Partido> PartidoIdEquipoVisitanteNavigations { get; set; } = new List<Partido>();
}
