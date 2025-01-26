using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;
using TC.Application.Exceptions;

namespace TC.Persistence.Command
{
    public class UpdateTicketsCatalogCommand : IUpdateTicketsCatalogCommand
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public UpdateTicketsCatalogCommand(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task ExecuteAsync(TicketsCatalog ticketsCatalog, int userId)
        {
            var catalog = await _context.Find(tc => tc.Id == ticketsCatalog.Id).FirstOrDefaultAsync();

            if (catalog.Owner.Id != userId)
                throw new AccessDeniedException();

            var update = Builders<TicketsCatalog>.Update
                .PullFilter(tc => tc.Users, user => user.Id == userId);

            await _context.ReplaceOneAsync(tc => tc.Id == ticketsCatalog.Id, ticketsCatalog);
        }

    }
}
