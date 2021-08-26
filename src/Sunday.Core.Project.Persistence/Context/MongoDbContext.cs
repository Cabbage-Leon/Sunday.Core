using MongoDB.Driver;
using Sunday.Core.Infrastructure;

namespace Sunday.Core.Persistence.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbContext()
        {
            var client = new MongoClient(Appsettings.App(new string[] { "Mongo", "ConnectionString" }));
            _database = client.GetDatabase(Appsettings.App(new string[] { "Mongo", "Database" }));
        }

        public IMongoDatabase Db
        {
            get { return _database; }
        }

        //public IMongoCollection<TEntity> Query
        //{
        //    get
        //    {
        //        return _database.GetCollection<TEntity>(nameof(TEntity));
        //    }
        //}
    }
}