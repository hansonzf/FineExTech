namespace Warehouse.Domain.AggregatesModel.StockInPaperAggregate
{
    public interface IStockInPaperRepository
    {
        Task<long> CreateStockInPaperAsync(StockInPaper paper);
        Task<bool> SaveStockInPaperAsync(StockInPaper paper);
        Task<StockInPaper?> GetStockInPaperAsync(string serialNumber, int tenantId);
        Task<bool> SetStockInPaperToProcessingAsync(string serialNumber, int tenantId);
        Task<IEnumerable<StockInPaper>> GetStandbyPaperByWarehouse(int tenantId, long storehouseId);
        Task<IEnumerable<StockInPaper>> GetStandbyPaperByCargoOwner(int tenantId, long cargoOwnerId);
    }
}
