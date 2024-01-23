using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityLogController: Controller
{
    private readonly IActivityLogRepository _activityLogRepository;

    public ActivityLogController(IActivityLogRepository activityLogRepository)
    {
        _activityLogRepository = activityLogRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ActivityLog))]
    public IActionResult GetActivityLogs()
    {
        var activityLogs = _activityLogRepository.GetActivityLogs();
        return Ok(activityLogs);
    }
}