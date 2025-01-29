using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;
using TC.Application.Exceptions;

namespace TC.Persistence.Command
{
    public class DeleteUserFromCatalogCommand : IDeleteUserFromCatalogCommand
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public DeleteUserFromCatalogCommand(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task ExecuteAsync(int catalogId, int userId, int userToRemoveId)
        {
            var catalog = await _context.Find(tc => tc.Id == catalogId).FirstOrDefaultAsync();

            if (catalog.Owner.Id != userId)
                throw new AccessDeniedException();

            var update = Builders<TicketsCatalog>.Update
                .PullFilter(tc => tc.Users, user => user.Id == userToRemoveId);

            await _context.UpdateOneAsync(tc => tc.Id == catalogId, update);
        }
    }
}
