using MediatR;
using Warehouse.Domain.AggregatesModel.StockInAggregate;
using Warehouse.Domain.Events;

namespace Warehouse.Api.Application.DomainEventHandlers.CountingArrivedCargo
{
    public class FillStockInPaperHandler
        : INotificationHandler<CountingArrivedCargoDomainEvent>
    {
        private readonly IStockInPaperRepository _repository;

        public FillStockInPaperHandler(IStockInPaperRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(CountingArrivedCargoDomainEvent notification, CancellationToken cancellationToken)
        {
            var paper = await _repository.GetStockInPaperAsync(notification.PaperSerialNumber, notification.TenantId);
            paper.Tally(notification.CargoCountingResult);
            await _repository.SaveStockInPaperAsync(paper);
        }
    }
}
