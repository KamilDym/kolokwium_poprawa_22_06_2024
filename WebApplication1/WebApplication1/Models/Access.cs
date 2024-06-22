using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Access
{
    public int IdUser { get; set; }

    public int IdProject { get; set; }

    public int? ColumnName { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
