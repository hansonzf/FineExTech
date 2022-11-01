using MediatR;
using Shippment.Domain.AggregateModels.ItineraryAggregate;
using Shippment.Domain.Events;

namespace Shipment.Api.Application.DomainEventHandlers.AcceptTransportOrder
{
    public class GenerateItineraryHandler
        : INotificationHandler<AcceptTransportOrderDomainEvent>
    {
        private readonly ItineraryRepository _repository;
        private readonly ILogger<GenerateItineraryHandler> _logger;

        public GenerateItineraryHandler(ItineraryRepository repository, ILogger<GenerateItineraryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(AcceptTransportOrderDomainEvent notification, CancellationToken cancellationToken)
        {
            string trackingNumber = notification.TrackingNumber;
            long orderId = notification.TransportOrderId;

            if (string.IsNullOrEmpty(trackingNumber))
            {
                _logger.LogError($"The transport order which Id {orderId} contains empty tracking number!");
                return;
            }

            var itinerary = new Itinerary(trackingNumber);
            bool result = await _repository.CreateNewItineraryAsync(itinerary);

            if (!result)
            {
                _logger.LogError($"The order which tracking number {trackingNumber} generate intinerary occured error!");
            }
        }
    }
}
