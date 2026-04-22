using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class EstadisticasPortero
{
    public int IdBase { get; set; }

    public virtual EstadisticasBase IdBaseNavigation { get; set; } = null!;
}
