using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public class ShelfCargoResult
    {
        public bool EntirellyShelfed => !UnableStockCargoDetails.Any();
        public List<CountingCargoResult> UnableStockCargoDetails { get; set; }
        public List<ShelfDetail> ShelfDetails { get; set; }
    }

    public class ShelfDetail : CountingCargoResult
    {
        public ShelfDetail(string locationCode, long cargoOwner, string sKU, int shelfCount, float shelfVolume)
            : base(cargoOwner, sKU, shelfCount, shelfVolume)
        {
            LocationCode = locationCode;
        }

        public string LocationCode { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LocationCode;
            yield return CargoOwner;
            yield return SKU;
            yield return Count;
            yield return Volume;
        }
    }
}
