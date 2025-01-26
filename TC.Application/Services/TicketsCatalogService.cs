using TC.Application.Interfaces;
using TC.Application.Entities;

namespace TC.Application.Services
{
    public class TicketsCatalogService(
        ICreateTicketsCatalogCommand createTicketsCatalogCommand,
        IUpdateTicketsCatalogCommand updateTicketsCatalogCommand,
        IDeleteTicketsCatalogCommand deleteTicketsCatalogCommand,
        IAddUserToCatalogCommand addUserToCatalogCommand,
        IDeleteUserFromCatalogCommand deleteUserFromCatalogCommand,
        IGetAllTicketsCatalogsQuery getAllTicketsCaalogsQuery,
        IGetTicketsCatalogByIdQuery getTicketsCatalogByIdQuery,
        IGetUsersByCatalogIdQuery getUsersByCatalogIdQuery) : ITicketsCatalogService
    {
        public async Task CreateCatalogAsync(TicketsCatalog ticketsCatalog, int userId)
        {
            await createTicketsCatalogCommand.ExecuteAsync(ticketsCatalog, userId);
        }

        public async Task UpdateCatalogAsync(TicketsCatalog ticketsCatalog, int userId)
        {
            await updateTicketsCatalogCommand.ExecuteAsync(ticketsCatalog, userId);
        }

        public async Task DeleteCatalogAsync(int catalogId, int userId)
        {
            await deleteTicketsCatalogCommand.ExecuteAsync(catalogId, userId);
        }

        public async Task<TicketsCatalog> GetCatalogByIdAsync(int catalogId, int userId)
        {
            return await getTicketsCatalogByIdQuery.ExecuteAsync(catalogId, userId);
        }

        public async Task<List<TicketsCatalog>> GetAllCatalogsAsync(int userId)
        {
            return await getAllTicketsCaalogsQuery.ExecuteAsync(userId);
        }
        public async Task AddUserToCatalogAsync(int catalogId, int userId, User userToAdd)
        {
            await addUserToCatalogCommand.ExecuteAsync(catalogId, userId, userToAdd);
        }

        public async Task<List<User>> GetAllUsersByCatalogIdAsync(int catalogId, int userId)
        {
            return await getUsersByCatalogIdQuery.ExecuteAsync(catalogId, userId);
        }

        public async Task DeleteUserFromCatalogAsync(int catalogId, int userId, int userToRemoveId)
        {
            await deleteUserFromCatalogCommand.ExecuteAsync(catalogId, userId, userToRemoveId);
        }
    }
}