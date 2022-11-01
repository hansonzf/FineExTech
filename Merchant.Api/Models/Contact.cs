using DomainBase;

namespace Merchants.Api.Models
{
    public class Contact : ValueObject
    {
        public Contact(string name, string contactWay, string number, string address, bool isIncharge = false)
        {
            Name = name;
            ContactWay = contactWay;
            Number = number;
            Address = address;
            IsIncharge = isIncharge;
        }

        public string Name { get; private set; }
        public string ContactWay { get; private set; }
        public string Number { get; private set; }
        public string Address { get; private set; }
        public bool IsIncharge { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ContactWay;
            yield return Number;
            yield return Address;
            yield return IsIncharge;
        }
    }
}
