using Orderpool.Api.Models.RemoteOrderAggregate;
using Orderpool.Api.Services;

namespace Orderpool.Api.Application.CollaborateServices
{
    public class OrderCenterService : IOrderCenterSerivce
    {
        private readonly IOrderCenterHttpAdapter _adapter;

        public OrderCenterService(IOrderCenterHttpAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task<IEnumerable<RemoteOrder>> PullOrder(DateTime orderBeforeTime)
        {
            throw new NotImplementedException();
        }
    }
}
