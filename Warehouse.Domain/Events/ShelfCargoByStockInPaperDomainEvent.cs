using MediatR;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Domain.Events
{
    public class ShelfCargoByStockInPaperDomainEvent : INotification
    {
        public ShelfCargoByStockInPaperDomainEvent(int tenantId, string paperSerialNumber, ShelfCargoResult shelfResult)
        {
            TenantId = tenantId;
            PaperSerialNumber = paperSerialNumber;
            ShelfResult = shelfResult;
        }

        public int TenantId { get; private set; }
        public string PaperSerialNumber { get; private set; }
        public ShelfCargoResult ShelfResult { get; }
        public long StorehouseId { get; private set; }
    }
}
