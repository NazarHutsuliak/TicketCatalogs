using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;
using TC.Application.Exceptions;

namespace TC.Persistence.Command
{
    public class DeleteTicketsCatalogCommand : IDeleteTicketsCatalogCommand
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public DeleteTicketsCatalogCommand(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task ExecuteAsync(int catalogId, int userId)
        {
            var catalog = await _context.Find(tc => tc.Id == catalogId).FirstOrDefaultAsync();

            if (catalog.Owner.Id != userId)
                throw new AccessDeniedException();

            await _context.DeleteOneAsync(tc => tc.Id == catalogId);

        }
    }
}
