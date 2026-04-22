using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class User
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string Contrasena { get; set; } = null!;

    public string? NombreUsuario { get; set; }

    public string? Correo { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;
}
