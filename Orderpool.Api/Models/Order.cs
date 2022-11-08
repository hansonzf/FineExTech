using DomainBase;

namespace Orderpool.Api.Models
{
    public class Order : ValueObject
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
