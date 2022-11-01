using MediatR;
using Orderpool.Api.Models.OrderWatcherAggregate;

namespace Orderpool.Api.Events
{
    public class OrderStartProcessingDomainEvent : INotification
    {
        public long OrderWatcherId { get; private set; }

        public OrderStartProcessingDomainEvent(long orderWatcherId)
        {
            OrderWatcherId = orderWatcherId; 
        }
    }
}
