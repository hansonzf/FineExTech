using MediatR;
using Warehouse.Domain.AggregatesModel.InventoryAggregate;
using Warehouse.Domain.AggregatesModel.StockInAggregate;
using Warehouse.Domain.Events;

namespace Warehouse.Api.Application.DomainEventHandlers.ShelfCargoByStockInPaper
{
    public class UpdateDomainDataHandler
        : INotificationHandler<ShelfCargoByStockInPaperDomainEvent>
    {
        private readonly IStockInPaperRepository _stockinpapaerRepository;
        private readonly IInventoryRecordRepository _inventoryrecordRepository;

        public UpdateDomainDataHandler(
            IStockInPaperRepository stockinpapaerRepository, 
            IInventoryRecordRepository inventoryrecordRepository)
        {
            _stockinpapaerRepository = stockinpapaerRepository;
            _inventoryrecordRepository = inventoryrecordRepository;
        }


        public async Task Handle(ShelfCargoByStockInPaperDomainEvent notification, CancellationToken cancellationToken)
        {
            var stockinpapaer = await _stockinpapaerRepository.GetStockInPaperAsync(notification.PaperSerialNumber, notification.TenantId);
            stockinpapaer.CompleteProcessing(notification.ShelfResult);
            await _stockinpapaerRepository.SaveStockInPaperAsync(stockinpapaer);

            foreach (var d in notification.ShelfResult.ShelfDetails)
            {
                var invRec = await _inventoryrecordRepository.GetRecordForOwnerSKUAsync(notification.StorehouseId, d.CargoOwner, d.LocationCode, d.SKU);
                if (invRec is null)
                    invRec = InventoryRecord.GetInstance(notification.StorehouseId, d.CargoOwner, d.LocationCode, d.SKU);
                invRec.StockIn(d.Count);

                await _inventoryrecordRepository.SaveInventoryRecordAsync(invRec);
            }
        }
    }
}
