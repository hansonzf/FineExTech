using DomainBase;
using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.AggregatesModel.StockInAggregate;

namespace Warehouse.Infrastructure.Repositories
{
    public class StockInPaperRepository : IStockInPaperRepository
    {
        private readonly WarehouseContext _context;

        public StockInPaperRepository(WarehouseContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<long> CreateStockInPaperAsync(StockInPaper paper)
        {
            if (paper is not null && paper.IsValid())
            {
                var entry = _context.Set<StockInPaper>().Attach(paper);
                entry.State = EntityState.Added;
                await UnitOfWork.SaveEntitiesAsync();

                return paper.Id;
            }

            return 0;
        }

        public async Task<IEnumerable<StockInPaper>> GetStandbyPaperByCargoOwner(int tenantId, long cargoOwnerId)
        {
            return await _context.Set<StockInPaper>().AsNoTracking().Include(s => s.StockInDetails)
                .Where(s => s.TenantId == tenantId && s.CargoOwnerId == cargoOwnerId && s.Status == StockInStatus.Standby)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockInPaper>> GetStandbyPaperByWarehouse(int tenantId, long storehouseId)
        {
            return await _context.Set<StockInPaper>().AsNoTracking().Include(s => s.StockInDetails)
                .Where(s => s.TenantId == tenantId && s.StockhouseId == storehouseId && s.Status == StockInStatus.Standby)
                .ToListAsync();
        }

        public async Task<StockInPaper?> GetStockInPaperAsync(string serialNumber, int tenantId)
        {
            if ((string.IsNullOrEmpty(serialNumber)) || (tenantId <= 0))
                return null;

            return await _context.Set<StockInPaper>()
                .AsNoTracking().Include(s => s.StockInDetails)
                .Where(s => s.PaperSerialNumber == serialNumber && s.TenantId == tenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveStockInPaperAsync(StockInPaper paper)
        {
            if (paper is not null && paper.IsValid())
            {
                var entry = _context.Set<StockInPaper>().Attach(paper);
                entry.State = EntityState.Modified;

                foreach (var item in paper.StockInDetails)
                {
                    string sql = $@"
UPDATE StockInDetail SET FactCount={item.FactCount}, StorehouseShelfedCount={item.StorehouseShelfedCount}, Remark='{item.Remark}'
WHERE Id={item.Id}
";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }

                await UnitOfWork.SaveEntitiesAsync();
                
                return true;
            }

            return false;
        }

        public async Task<bool> SetStockInPaperToProcessingAsync(string serialNumber, int tenantId)
        {
            if ((string.IsNullOrEmpty(serialNumber)) || (tenantId <= 0))
                return false;

            int affectRows = await _context.Database.ExecuteSqlRawAsync($@"
UPDATE StockInPaper 
SET Status = 'Processing' 
WHERE TenantId = {tenantId} and PaperSerialNumber = '{serialNumber}'
");

            return affectRows == 1;
        }
    }
}
