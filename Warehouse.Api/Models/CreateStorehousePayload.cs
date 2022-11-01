namespace Warehouse.Api.Models
{
    public class CreateStorehousePayload
    {
        public int TenantId { get; set; }
        public long MerchantId { get; set; }
        public string StorehouseName { get; set; }
    }
}
