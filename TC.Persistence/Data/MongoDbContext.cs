using MongoDB.Bson;
using MongoDB.Driver;
using TC.Application.Entities;

namespace TC.Persistence.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string? connectionString, string? databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<TicketsCatalog> TicketCatalogs => _database.GetCollection<TicketsCatalog>("TicketsCatalog");
    }
}
