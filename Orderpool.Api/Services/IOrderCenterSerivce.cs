using Orderpool.Api.Models;

namespace Orderpool.Api.Services
{
    public interface IOrderCenterSerivce
    {
        Task<IEnumerable<Order>> PullOrder(DateTime orderBeforeTime);
    }
}
