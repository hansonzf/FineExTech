using DomainBase;

namespace LocationApi.Domain
{
    public class Address : ValueObject
    {
        public string Country { get; private set; }
        public string Province { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set;  }
        public string PostalCode { get; private set; }
        public string DetailAddress { get; private set;  }

        private Address()
        { }

        public Address(string country, string province, string city, string street, string postalCode, string detail)
        {
            Country = country;
            Province = province;
            City = city;
            Street = street;
            PostalCode = postalCode;
            DetailAddress = detail;
        }

        public static Address Null()
        {
            var addr = new Address
            {
                Country = default,
                Province = default,
                City = default,
                Street = default,
                PostalCode = default,
                DetailAddress = default
            };

            return addr;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return Province;
            yield return City;
            yield return Street;
            yield return PostalCode;
            yield return DetailAddress;
        }
    }
}
