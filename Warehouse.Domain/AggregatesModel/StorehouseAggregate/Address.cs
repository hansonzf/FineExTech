using DomainBase;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public class Address : ValueObject
    {
        private Address()
        { }

        public Address(string province, string city, string region, string detailAddress)
        {
            Province = province;
            City = city;
            Region = region;
            DetailAddress = detailAddress;
        }

        public string Province { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string DetailAddress { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Province;
            yield return City;
            yield return Region;
            yield return DetailAddress;
        }

        public static Address Empty => new Address();
    }
}
