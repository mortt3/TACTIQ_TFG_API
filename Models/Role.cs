using System;
using System.Collections.Generic;

namespace TactiqApi.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string Rol { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
