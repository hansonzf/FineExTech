using Orderpool.Api.Models.OrderWatcherAggregate;
using Orderpool.Api.Models.RemoteOrderAggregate;

namespace Orderpool.Api.Models
{
    public class ImportOrderService
    {
        private readonly IRemoteOrderRepository _remoteOrderRepository;
        private readonly IOrderWatcherRepository _orderWatcherRepository;

        public ImportOrderService(IRemoteOrderRepository remoteOrderRepository, IOrderWatcherRepository orderWatcherRepository)
        {
            _remoteOrderRepository = remoteOrderRepository;
            _orderWatcherRepository = orderWatcherRepository;
        }

        public async Task<int> ImportOrder(List<RemoteOrder> orders)
        {
            if (!orders.Any())
                return 0;

            List<OrderWatcher> watcherList = new List<OrderWatcher>();

            foreach (var item in orders)
            {
                Guid orderUuid = item.Bind();
                var orderWatcher = new OrderWatcher(item.OriginOrderPK, orderUuid);
                watcherList.Add(orderWatcher);
            }

            int importedCount = await _remoteOrderRepository.BulkInsert(orders);
            await _orderWatcherRepository.BulkInsert(watcherList);

            return importedCount;
        }
    }
}
