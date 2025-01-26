﻿using TC.Application.Entities;
using TC.Persistence.Data;
using MongoDB.Driver;
using TC.Application.Interfaces;
using TC.Application.Exceptions;

namespace TC.Persistence.Query
{
    public class GetTicketsCatalogByIdQuery : IGetTicketsCatalogByIdQuery
    {
        private readonly IMongoCollection<TicketsCatalog> _context;

        public GetTicketsCatalogByIdQuery(MongoDbContext context)
        {
            _context = context.TicketCatalogs;
        }

        public async Task<TicketsCatalog> ExecuteAsync(int catalogId, int userId)
        {
            var catalog = await _context.Find(tc => tc.Id == catalogId).FirstOrDefaultAsync();

            if (catalog.Owner.Id != userId && !catalog.Users.Any(user => user.Id == userId))
                throw new AccessDeniedException();

            return catalog;
        }
    }
}
