using DomainBase;
using Orderpool.Api.Models.RemoteOrderAggregate;

namespace Orderpool.Api.Infrastructure
{
    public class RemoteOrderRepository : IRemoteOrderRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<int> BulkInsert(List<RemoteOrder> remoteOrders)
        {
            throw new NotImplementedException();
        }
    }
}
