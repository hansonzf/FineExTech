using DomainBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.AggregatesModel.InventoryAggregate;

namespace Warehouse.Infrastructure.Repositories
{
    public class InventoryRecordRepository : IInventoryRecordRepository
    {
        private readonly WarehouseContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public InventoryRecordRepository(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<InventoryRecord?> GetRecordForOwnerSKUAsync(long storehouseId, long ownerId, string locationCode, string sku)
        {
            return await _context.Set<InventoryRecord>()
                .Where(i => i.StorehouseId == storehouseId
                    && i.CargoOwner == ownerId
                    && i.LocationCode == locationCode
                    && i.SKU == sku)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<InventoryRecord>> GetRecordsForStorehouseAsync(long storehouseId)
        {
            return await _context.Set<InventoryRecord>()
                .Where(i => i.StorehouseId == storehouseId)
                .ToListAsync();
        }

        public async Task<bool> SaveInventoryRecordAsync(InventoryRecord record)
        {
            var entry = _context.Set<InventoryRecord>().Attach(record);
            entry.State = record.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await UnitOfWork.SaveEntitiesAsync();
        }
    }
}
