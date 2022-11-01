using MediatR;
using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;

namespace Warehouse.Domain.Events
{
    public class CountingArrivedCargoDomainEvent : INotification
    {
        public CountingArrivedCargoDomainEvent(string paperSerialNumber, int tenantId, List<CountingCargoResult> cargoCountingResult)
        {
            PaperSerialNumber = paperSerialNumber;
            TenantId = tenantId;
            CargoCountingResult = cargoCountingResult;
        }

        public string PaperSerialNumber { get; private set; }
        public int TenantId { get; private set; }
        public List<CountingCargoResult> CargoCountingResult { get; private set; }
    }
}
