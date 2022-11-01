using DomainBase;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public enum StockAreaType
    {
        Unknow = 0,
        General,
        LowTemperature,
        Freeze
    }

    public class StockArea : Entity
    {
        const string TAGSEPARATE = ",";
        internal StockArea() 
        { }

        internal StockArea(long storehouseId, string locationCode, StockAreaType type, string locationName, float maxVolume)
        {
            StorehouseId = storehouseId;
            Code = locationCode;
            LocationName = locationName;
            MaxLoad = maxVolume;
            UsedLoad = 0;
            Useable = true;
            Type = type;
        }

        public long StorehouseId { get; protected set; }
        public string Code { get; protected set; }
        public string LocationName { get; protected set; }
        public string Tags { get; protected set; }
        public float MaxLoad { get; protected set; }
        public float UsedLoad { get; protected set; }
        public bool Useable { get; protected set; }
        public bool AllowOverload { get; protected set; }
        public StockAreaType Type { get; protected set; }
        public float LeftLoad => MaxLoad - UsedLoad;
        public string[] TagList => Tags.Split(TAGSEPARATE);

        public void DisableLocation()
        {
            if (UsedLoad > 0)
                throw new InvalidOperationException("the stock area still contains cargo!");

            Useable = false;
        }

        public void EnableLocation()
        {
            Useable = true;
        }

        public bool Shelf(float cargoCount)
        {
            if (!Useable)
                return false;

            if (UsedLoad + cargoCount > MaxLoad && !AllowOverload)
                return false;

            UsedLoad += cargoCount;

            return true;
        }
    }
}
