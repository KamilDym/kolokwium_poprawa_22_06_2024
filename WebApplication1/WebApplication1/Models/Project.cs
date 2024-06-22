using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public string Name { get; set; } = null!;

    public int IdDefaultAssignee { get; set; }

    public virtual ICollection<Access> Accesses { get; set; } = new List<Access>();

    public virtual User IdDefaultAssigneeNavigation { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
