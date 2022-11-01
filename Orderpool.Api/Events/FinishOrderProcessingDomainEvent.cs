using MediatR;

namespace Orderpool.Api.Events
{
    public class FinishOrderProcessingDomainEvent : INotification
    {
        public long WatcherId { get; set; }
        public FinishOrderProcessingDomainEvent(long watcherId)
        {
            WatcherId = watcherId;
        }
    }
}
