using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain.AggregatesModel.InventoryAggregate
{
    public interface IInventoryRecordRepository
    {
        Task<IEnumerable<InventoryRecord>> GetRecordsForStorehouseAsync(long storehouseId);
        Task<InventoryRecord> GetRecordForOwnerSKUAsync(long storehouseId, long ownerId, string locationCode, string sku);
        Task<bool> SaveInventoryRecordAsync(InventoryRecord record);
    }
}
