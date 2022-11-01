using MediatR;

namespace Warehouse.Domain.Events
{
    public class StockInPaperArrivedDomainEvent : INotification
    {
        public StockInPaperArrivedDomainEvent(int tenantId, long storehouseId, string paperNumber)
        {
            TenantId = tenantId;
            StorehouseId = storehouseId;
            PaperNumber = paperNumber;
        }

        public int TenantId { get; set; }
        public long StorehouseId { get; set; }
        public string PaperNumber { get; set; }
    }
}
