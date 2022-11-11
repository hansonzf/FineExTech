using DomainBase;

namespace LocationApi.Domain.AggregateModels.LocationAggregate
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<Location> GetAsync(long locationId);
        Task<IEnumerable<Location>> GetByOwnerAsync(long ownerId, int pageIndex = 1, int pageSize = 20);
        Task<long> CreateAsync(Location location);
    }
}
