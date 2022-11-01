using Orderpool.Api.Models.RemoteOrderAggregate;

namespace Orderpool.Api.Services
{
    public interface IOrderCenterSerivce
    {
        Task<IEnumerable<RemoteOrder>> PullOrder(DateTime orderBeforeTime);
    }

    public interface IOrderCenterHttpAdapter
    {
        Task QueryOrders(DateTime orderBeforeTime);
    }
}
