using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class Posicion
{
    public int IdPosicion { get; set; }

    public string Categoria { get; set; } = null!;

    public string RolEspecifico { get; set; } = null!;

    public virtual ICollection<Jugadore> Jugadores { get; set; } = new List<Jugadore>();
}
