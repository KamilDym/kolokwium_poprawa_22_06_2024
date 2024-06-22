using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOs;
using WebApplication1.Models;
using Task = WebApplication1.Models.Task;

namespace WebApplication1.Controllers;



[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly S25005Context _context;

    public TaskController(S25005Context context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskDTO>> GetTasks([FromQuery] int? projectId)
    {
        try
        {
            List<Task> tasks;

            if (projectId.HasValue)
            {
                tasks = _context.Tasks
                                .Include(t => t.IdReporterNavigation)
                                .Include(t => t.IdAssigneeNavigation)
                                .Where(t => t.IdProject == projectId.Value)
                                .ToList();
                if (!tasks.Any())
                {
                    return NotFound("No tasks found for the given project ID.");
                }
            }
            else
            {
                tasks = _context.Tasks
                                .Include(t => t.IdReporterNavigation)
                                .Include(t => t.IdAssigneeNavigation)
                                .ToList();
            }

            var taskDTOs = tasks.Select(t => new TaskDTO
            {
                IdTask = t.IdTask,
                Name = t.Name,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                IdProject = t.IdProject,
                IdReporter = t.IdReporter,
                Reporter = new ReporterDTO
                {
                    FirstName = t.IdReporterNavigation.FirstName,
                    LastName = t.IdReporterNavigation.LastName
                },
                IdAssignee = t.IdAssignee,
                Assignee = new AssigneeDTO
                {
                    FirstName = t.IdAssigneeNavigation.FirstName,
                    LastName = t.IdAssigneeNavigation.LastName
                }
            }).ToList();

            return Ok(new { tasks = taskDTOs });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(requestDto.IdProject);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            var reporter = await _context.Users.FindAsync(requestDto.IdReporter);
            if (reporter == null)
            {
                return NotFound("Reporter not found.");
            }

            User assignee = null;
            if (requestDto.IdAssignee.HasValue)
            {
                assignee = await _context.Users.FindAsync(requestDto.IdAssignee.Value);
                if (assignee == null)
                {
                    return NotFound("Assignee not found.");
                }
            }

            var reporterAccess = await _context.Accesses
                .AnyAsync(a => a.IdProject == requestDto.IdProject && a.IdUser == requestDto.IdReporter);
            if (!reporterAccess)
            {
                return Forbid("Reporter does not have access to the project.");
            }

            if (assignee != null)
            {
                var assigneeAccess = await _context.Accesses
                    .AnyAsync(a => a.IdProject == requestDto.IdProject && a.IdUser == requestDto.IdAssignee.Value);
                if (!assigneeAccess)
                {
                    return Forbid("Assignee does not have access to the project.");
                }
            }
            else
            {
                assignee = await _context.Users.FindAsync(project.IdDefaultAssignee);
                if (assignee == null)
                {
                    return NotFound("Default assignee not found.");
                }
            }

            var task = new Task
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                CreatedAt = DateTime.UtcNow,
                IdProject = requestDto.IdProject,
                IdReporter = requestDto.IdReporter,
                IdAssignee = assignee.IdUser
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Created();
        }
}

