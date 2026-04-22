using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TactiqApi.Models;

public partial class Jugadore
{
    public int IdJugador { get; set; }

    public string NombreJugador { get; set; } = null!;

    public int Dorsal { get; set; }

    public string? ImagenJugador { get; set; }

    public int? IdPosicion { get; set; }

    public int? IdEquipo { get; set; }
    
    [Column("edad")] // Esto le dice a Postgres que use "edad" en minúsculas
    public int? edad { get; set; }

    public virtual ICollection<EstadisticasBase> EstadisticasBases { get; set; } = new List<EstadisticasBase>();

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual Posicion? IdPosicionNavigation { get; set; }

}
