using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;
using TC.Application.Exceptions;

namespace TC.Persistence.Command
{
    public class AddUserToCatalogCommand : IAddUserToCatalogCommand
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public AddUserToCatalogCommand(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task ExecuteAsync(int catalogId, int userId, User userToAdd)
        {
            var catalog = await _context.Find(tc => tc.Id == catalogId).FirstOrDefaultAsync();

            if (catalog.Owner.Id != userId)
                throw new AccessDeniedException();

            if (catalog.Users.Any(user => user.Id == userToAdd.Id))
                throw new AlreadyExistException();

            var update = Builders<TicketsCatalog>.Update.Push(tc => tc.Users, userToAdd);
            await _context.UpdateOneAsync(tc => tc.Id == catalogId, update);
        }
    }
}
