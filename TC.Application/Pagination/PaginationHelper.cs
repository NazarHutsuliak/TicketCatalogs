using MongoDB.Driver;
using MongoDB.Driver.Linq;
namespace TC.Application.Pagination
{
    public static class PaginationHelper
    {
        public static PaginatedResult<T> Paginate<T>(
            List<T> data,
            int currentPage,
            int pageSize)
        {
            var totalCount = data.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedData = data
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<T>
            {
                Data = paginatedData,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}

