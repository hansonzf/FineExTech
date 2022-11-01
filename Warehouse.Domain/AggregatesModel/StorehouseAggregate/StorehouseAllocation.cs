using DomainBase;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public class StorehouseAllocation : ValueObject
    {
        public StorehouseAllocation(string code, string name, float capability, StockAreaType areaType)
        {
            Code = code;
            Name = name;
            MaxCapability = capability;
            AreaType = areaType;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public float MaxCapability { get; private set; }
        public StockAreaType AreaType { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
            yield return Name;
            yield return MaxCapability;
        }
    }
}
