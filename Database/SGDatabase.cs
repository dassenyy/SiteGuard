using System.Collections.Generic;
using System.Linq;
using LiteDB;
using SiteGuard.DataModels.DatabaseSchemas;

namespace SiteGuard.Database {
    public class SGDatabase {
        private readonly string _connectionString;
        
        private const string _userCollection = "users";
        private const string _reportCollection = "reports";
        private const string _warnCollection = "warns";
        private const string _kickCollection = "kicks";
        private const string _idBanCollection = "id_bans";
        private const string _ipBanCollection = "ip_bans";
        
        /// <summary>
        /// Creates a new instance using the provided connection string, which will be used for all database operations.
        /// </summary>
        /// <param name="absoluteDatabasePath">Absolute path to the database.</param>
        public SGDatabase(string absoluteDatabasePath) {
            _connectionString = absoluteDatabasePath;
        }
        
        public BsonValue UpsertUser(User user) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<User> collection = connection.GetCollection<User>(_userCollection);
                
                // In this case upsert the user instead of inserting them to make sure that their name is always up-to-date.
                return collection.Upsert(user);
            }
        }
        
        public User GetUser(string userId) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<User> collection = connection.GetCollection<User>(_userCollection);
                return collection.FindById(userId);
            }
        }
        
        public BsonValue InsertReport(Report report) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<Report> collection = connection.GetCollection<Report>(_reportCollection);
                return collection.Insert(report);
            }
        }
        
        public List<Report> GetReportsByTargetId(string reportedId) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<Report> collection = connection.GetCollection<Report>(_reportCollection);
                return collection.Find(element => element.TargetId == reportedId).ToList();
            }
        }
        
        public BsonValue InsertWarn(Warn warn) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<Warn> collection = connection.GetCollection<Warn>(_warnCollection);
                return collection.Insert(warn);
            }
        }
        
        public List<Warn> GetWarnsByTargetId(string targetId) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<Warn> collection = connection.GetCollection<Warn>(_warnCollection);
                return collection.Find(element => element.TargetId == targetId).ToList();
            }
        }
        
        public BsonValue InsertKick(Kick kick) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<Kick> collection = connection.GetCollection<Kick>(_kickCollection);
                return collection.Insert(kick);
            }
        }
        
        public List<Kick> GetKicksByTargetId(string targetId) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<Kick> collection = connection.GetCollection<Kick>(_kickCollection);
                return collection.Find(element => element.TargetId == targetId).ToList();
            }
        }
        
        public BsonValue InsertIdBan(IdBan idBan) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<IdBan> collection = connection.GetCollection<IdBan>(_idBanCollection);
                return collection.Insert(idBan);
            }
        }
        
        public List<IdBan> GetIdBansByTargetId(string targetId) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<IdBan> collection = connection.GetCollection<IdBan>(_idBanCollection);
                return collection.Find(element => element.TargetId == targetId).ToList();
            }
        }
        
        public BsonValue InsertIPBan(IPBan ipBan) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<IPBan> collection = connection.GetCollection<IPBan>(_ipBanCollection);
                return collection.Insert(ipBan);
            }
        }
        
        public List<IPBan> GetIPBansByTargetIP(string ip) {
            using (LiteDatabase connection = new LiteDatabase(_connectionString)) {
                ILiteCollection<IPBan> collection = connection.GetCollection<IPBan>(_ipBanCollection);
                return collection.Find(element => element.TargetIP == ip).ToList();
            }
        }
    }
}