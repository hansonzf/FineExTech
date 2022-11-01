using DomainBase;

namespace Orderpool.Api.Models.RemoteOrderAggregate
{
    public interface IRemoteOrderRepository : IRepository<RemoteOrder>
    {
        Task<int> BulkInsert(List<RemoteOrder> remoteOrders);
    }
}
