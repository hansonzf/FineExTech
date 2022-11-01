namespace Warehouse.Api.Models
{
    public class ShelfCargoPayload
    {
        public long StorehouseId { get; set; }
        public string StockinPaperSerialNumber { get; set; }
        public List<CountingResult> UnableShelfedCargo { get; set; }
        public List<ShelfResult> ShelfedCargo { get; set; }
    }

    public class ShelfResult : CountingResult
    {
        public string LocationCode { get; set; }
    }
}
