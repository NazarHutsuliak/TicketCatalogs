using TC.Application.Entities;

namespace TC.Application.Interfaces
{
    public interface ICreateTicketsCatalogCommand
    {
        Task ExecuteAsync(TicketsCatalog ticketsCatalog, int userId);
    }

    public interface IUpdateTicketsCatalogCommand
    {
        Task ExecuteAsync(TicketsCatalog ticketsCatalog, int userId);
    }

    public interface IDeleteTicketsCatalogCommand
    {
        Task ExecuteAsync(int catalogId, int userId);
    }

    public interface IAddUserToCatalogCommand
    {
        Task ExecuteAsync(int catalogId, int userId, User userToAdd);
    }

    public interface IDeleteUserFromCatalogCommand
    {
        Task ExecuteAsync(int catalogId, int userId, int userToRemoveId);
    }
}
