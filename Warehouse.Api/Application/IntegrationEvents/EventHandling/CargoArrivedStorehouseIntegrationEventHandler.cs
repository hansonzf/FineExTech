using EventBus.Interfaces;
using MediatR;
using Warehouse.Api.Application.IntegrationEvents.Events;
using Warehouse.Domain.AggregatesModel.StockInAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Api.Application.IntegrationEvents.EventHandling
{
    public class CargoArrivedStorehouseIntegrationEventHandler
        : IIntegrationEventHandler<CargoArrivedStorehouseIntegrationEvent>
    {
        private readonly IStockInPaperRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<CargoArrivedStorehouseIntegrationEventHandler> _logger;

        public CargoArrivedStorehouseIntegrationEventHandler(
            IMediator mediator,
            ILogger<CargoArrivedStorehouseIntegrationEventHandler> logger,
            IStockInPaperRepository repository)
        {
            _mediator = mediator;
            _logger = logger;
            _repository = repository;
        }


        public async Task Handle(CargoArrivedStorehouseIntegrationEvent @event)
        {
            var stockinPaper = StockInPaper.CreateInstance(
                @event.TenantId,
                @event.WarehouseId, 
                @event.CargoOwnerId, 
                @event.CargoOwnerName, 
                @event.CargoSummary.AsEnumerable().ToList());
            _ = await _repository.CreateStockInPaperAsync(stockinPaper);

            _logger.LogInformation($"Success processing CargoArrivedStorehouseEvent {@event.Id}");
        }
    }
}
