using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Models
{
    public class ActivityLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ownerId")]
        public int OwnerId { get; set; }
        [BsonElement("activity")]
        public string Activity { get; set; }
    }
}
