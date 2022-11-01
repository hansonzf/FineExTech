using MediatR;
using Warehouse.Domain.AggregatesModel.InventoryAggregate;
using Warehouse.Domain.Events;

namespace Warehouse.Api.Application.DomainEventHandlers.SetupStorehouse
{
    public class CreateInventoryBoardHandler
        : INotificationHandler<SetupStorehouseDomainEvent>
    {
        private readonly IInventoryRecordRepository _repository;

        public CreateInventoryBoardHandler(IInventoryRecordRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(SetupStorehouseDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
