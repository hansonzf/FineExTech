using DomainBase;

namespace Warehouse.Domain.AggregatesModel.InventoryAggregate
{
    public class InventoryRecord : Entity, IAggregateRoot
    {
        internal InventoryRecord()
        { }

        public static InventoryRecord GetInstance(long storehouseId, long cargoOwner, string sku, string locationCode)
        {
            var record = new InventoryRecord
            {
                StorehouseId = storehouseId,
                CargoOwner = cargoOwner,
                SKU = sku,
                LocationCode = locationCode,
                StockCount = 0,
                OpenCount = 0
            };

            return record;
        }

        public long StorehouseId { get; private set; }
        public long CargoOwner { get; private set; }
        public string SKU { get; private set; }
        public string LocationCode { get; private set; }
        public int StockCount { get; private set; }
        public int OpenCount { get; private set; }
        public int AvailableStockCount => StockCount - OpenCount;

        public void StockIn(int count)
        {
            StockCount += count;
        }
    }
}
