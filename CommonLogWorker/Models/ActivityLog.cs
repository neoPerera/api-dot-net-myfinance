using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommonLogWorker.Models
{
    public class ActivityLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("level")]
        public string Level { get; set; }

        [BsonElement("service")]
        public string Service { get; set; }

        [BsonElement("userId")]
        public string? UserId { get; set; }

        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;

        [BsonElement("message")]
        public string Message { get; set; } = string.Empty;

        [BsonElement("metadata")]
        public BsonDocument? Metadata { get; set; }
    }
}
