namespace WebApplication1.DTOs;

public class CreateTaskRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int IdProject { get; set; }
    public int IdReporter { get; set; }
    public int? IdAssignee { get; set; }
}