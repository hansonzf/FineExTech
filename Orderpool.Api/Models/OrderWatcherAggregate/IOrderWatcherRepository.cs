using DomainBase;

namespace Orderpool.Api.Models.OrderWatcherAggregate
{
    public interface IOrderWatcherRepository : IRepository<OrderWatcher>
    {
        Task<OrderWatcher?> FetchNextOrderWatcherAsync();
        Task<bool> SaveProcessAsync(OrderWatcher watcher);
        Task<OrderWatcher> GetByIdAsync(long id);
        Task<int> BulkInsert(List<OrderWatcher> orders);
        Task<bool> SaveAsync(OrderWatcher watcher);
    }
}
