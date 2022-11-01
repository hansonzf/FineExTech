using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Domain.Services
{
    public interface ICountingCargoService
    {
        Task<IEnumerable<StockArea>> CountingCargo(int tenantId, string stockinPaperNum, string countingPaperNum, List<CountingCargoResult> countingResult);
    }
}
