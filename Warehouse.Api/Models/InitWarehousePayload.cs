namespace Warehouse.Api.Models
{
    public class InitWarehousePayload
    {
        public long StorehouseId { get; set; }
        public IEnumerable<StoreLocationPlan> StoreLocationsAllocation { get; set; }
    }

    public class StoreLocationPlan
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Volume { get; set; }
    }
}
