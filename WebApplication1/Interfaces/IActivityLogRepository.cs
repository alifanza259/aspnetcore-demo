using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface IActivityLogRepository
{
    ICollection<ActivityLog> GetActivityLogs();

    bool CreateActivityLog(ActivityLog activityLog);
}