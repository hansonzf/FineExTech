using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;
using Warehouse.Domain.AggregatesModel.StockInPaperAggregate;

namespace Warehouse.Domain.Services
{
    public interface ICountingRepository
    {
        Task<StockInPaper> GetStockInPaper(int tenantId, string serialNumber);
        Task<CountingCargoPaper> GetCountingCargoPaper(int tenantId, string serialNumber);
        Task<bool> SaveCountingResult(StockInPaper stockinPaper, CountingCargoPaper countingCargoPaper);
    }
}
