using LiteDB;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public class User {
        [BsonId]
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}