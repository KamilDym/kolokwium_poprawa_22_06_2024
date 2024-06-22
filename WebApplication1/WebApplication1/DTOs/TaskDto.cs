namespace WebApplication1.DTOs;


public class TaskDTO
{
    public int IdTask { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int IdProject { get; set; }
    public int IdReporter { get; set; }
    public ReporterDTO Reporter { get; set; }
    public int IdAssignee { get; set; }
    public AssigneeDTO Assignee { get; set; }
}
