using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;

namespace TC.Persistence.Query
{
    public class GetAllTicketsCatalogsQuery : IGetAllTicketsCatalogsQuery
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public GetAllTicketsCatalogsQuery(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task<List<TicketsCatalog>> ExecuteAsync(int userId)
        {
            var filter = Builders<TicketsCatalog>.Filter.Or(
            Builders<TicketsCatalog>.Filter.Eq(t => t.Owner.Id, userId),
            Builders<TicketsCatalog>.Filter.ElemMatch(t => t.Users, user => user.Id == userId));
            
            return await _context.Find(filter).ToListAsync();
        }
    }
}
