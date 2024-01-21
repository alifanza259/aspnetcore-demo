using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class ActivityLogRepository: IActivityLogRepository
{
    private readonly IMongoCollection<ActivityLog> _activityLogCollection;

    public ActivityLogRepository(IOptions<ActivityDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);

        _activityLogCollection = database.GetCollection<ActivityLog>(settings.Value.ActivitiesCollectionName);
    }

    public ICollection<ActivityLog> GetActivityLogs()
    {
        return _activityLogCollection.Find(_ => true).ToList();
    }

    public bool CreateActivityLog(ActivityLog activityLog)
    {
        throw new NotImplementedException();
    }
}