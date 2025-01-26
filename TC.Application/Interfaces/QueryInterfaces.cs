using TC.Application.Entities;

namespace TC.Application.Interfaces
{
    public interface IGetTicketsCatalogByIdQuery
    {
        Task<TicketsCatalog> ExecuteAsync(int catalogId, int userId);
    }

    public interface IGetAllTicketsCatalogsQuery
    {
        Task<List<TicketsCatalog>> ExecuteAsync(int userId);
    }

    public interface IGetUsersByCatalogIdQuery 
    {
        Task<List<User>> ExecuteAsync(int catalogId, int userId);
    }
}
