using DomainBase;

namespace Shippment.Domain.AggregateModels.LocationAggregate
{
    public class LocationDescription : ValueObject
    {
        public long LocationId { get; private set; }
        public string LocationName { get; private set; }

        public LocationDescription(long locationId, string locationName)
        {
            LocationId = locationId;
            LocationName = locationName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LocationId;
            yield return LocationName;
        }
    }
}
