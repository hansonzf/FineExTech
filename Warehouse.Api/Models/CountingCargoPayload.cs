namespace Warehouse.Api.Models
{
    public class CountingCargoPayload
    {
        public long StorehouseId { get; set; }
        public string StockinPaperSerialNumber { get; set; }
        public List<CountingResult> Results { get; set; }
    }

    public class CountingResult
    {
        public long CargoOwner { get; set; }
        public string SKU { get; set; }
        public int Count { get; set; }
        public float Volume { get; set; }
    }
}
