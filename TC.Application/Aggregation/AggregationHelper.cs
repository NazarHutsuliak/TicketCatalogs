using TC.Application.Entities;

namespace TC.Application.Aggregation
{
    public class AggregationHelper
    {
        public static List<AggregationResult> AggregateCatalogs(List<TicketsCatalog> catalogs)
        {
            return catalogs.Select(catalog => new AggregationResult
            {
                Id = catalog.Id,
                Name = catalog.Name,
                DataTime = catalog.DataTime
            })
            .OrderByDescending(x => x.DataTime)
            .ToList();
        }
    }
}
