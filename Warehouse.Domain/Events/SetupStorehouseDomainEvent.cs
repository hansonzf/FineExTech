using MediatR;

namespace Warehouse.Domain.Events
{
    public class SetupStorehouseDomainEvent : INotification
    {
        public SetupStorehouseDomainEvent(int tenantId, long merchantId, long storehouseId)
        {
            TenantId = tenantId;
            MerchantId = merchantId;
            StorehouseId = storehouseId;
        }

        public int TenantId { get; private set; }
        public long MerchantId { get; private set; }
        public long StorehouseId { get; private set; }
    }
}
