using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;
using TC.Application.Exceptions;

namespace TC.Persistence.Command
{
    public class CreateTicketsCatalogCommand : ICreateTicketsCatalogCommand
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public CreateTicketsCatalogCommand(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task ExecuteAsync(TicketsCatalog ticketsCatalog, int userId)
        {
            await _context.InsertOneAsync(ticketsCatalog);
        }
    }
}
