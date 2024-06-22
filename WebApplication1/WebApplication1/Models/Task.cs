using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Task
{
    public int IdTask { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int IdProject { get; set; }

    public int IdReporter { get; set; }

    public int IdAssignee { get; set; }

    public virtual User IdAssigneeNavigation { get; set; } = null!;

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual User IdReporterNavigation { get; set; } = null!;
}
