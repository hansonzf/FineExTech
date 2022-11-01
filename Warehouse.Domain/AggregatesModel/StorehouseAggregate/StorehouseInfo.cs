using DomainBase;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public class StorehouseInfo : ValueObject
    {
        private StorehouseInfo() { }

        public StorehouseInfo(int tenantId, string tenantName, long merchantId, string warehouseName)
        {
            TenantId = tenantId;
            TenantName = tenantName;
            MerchantId = merchantId;
            WarehouseName = warehouseName;
        }

        public int TenantId { get; private set; }
        public string TenantName { get; private set; }
        public long MerchantId { get; private set; }
        public string WarehouseName { get; private set; }

        public static StorehouseInfo Empty => new StorehouseInfo();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TenantId;
            yield return TenantName;
            yield return MerchantId;
            yield return WarehouseName;
        }
    }
}
