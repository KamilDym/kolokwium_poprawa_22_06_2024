using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Access> Accesses { get; set; } = new List<Access>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Task> TaskIdAssigneeNavigations { get; set; } = new List<Task>();

    public virtual ICollection<Task> TaskIdReporterNavigations { get; set; } = new List<Task>();
}
