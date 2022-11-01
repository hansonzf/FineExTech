using DomainBase;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public interface IStorehouseRepository : IRepository<Storehouse>
    {
        Task<long> CreateStorehouseAsync(Storehouse storehouse);
        Task<bool> SaveStorehouseChangesAsync(Storehouse storehouse);
        Task<Storehouse?> GetStorehouseAsync(long id);
    }
}
