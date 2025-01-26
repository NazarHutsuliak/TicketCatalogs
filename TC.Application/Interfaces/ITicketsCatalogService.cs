using TC.Application.Entities;

namespace TC.Application.Interfaces
{
    public interface ITicketsCatalogService
    {
        Task CreateCatalogAsync(TicketsCatalog ticketsCatalog, int userId);
        Task UpdateCatalogAsync(TicketsCatalog ticketsCatalog, int userId);
        Task DeleteCatalogAsync(int catalogId, int userId);
        Task<TicketsCatalog> GetCatalogByIdAsync(int catalogId, int userId);
        Task<List<TicketsCatalog>> GetAllCatalogsAsync(int userId);
        Task AddUserToCatalogAsync(int catalogId, int userId, User userToAdd);
        Task<List<User>> GetAllUsersByCatalogIdAsync(int catalogId, int userId);
        Task DeleteUserFromCatalogAsync(int catalogId, int userId, int userToRemoveId);
    }
}
