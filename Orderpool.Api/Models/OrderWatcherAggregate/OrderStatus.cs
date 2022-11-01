using DomainBase;

namespace Orderpool.Api.Models.OrderWatcherAggregate
{
    public class OrderStatus : ValueObject
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
