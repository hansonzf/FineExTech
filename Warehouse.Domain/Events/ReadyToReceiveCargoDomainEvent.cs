using MediatR;

namespace Warehouse.Domain.Events
{
    public class ReadyToReceiveCargoDomainEvent : INotification
    {
        public ReadyToReceiveCargoDomainEvent(string paperNumber, int tenantId)
        {
            TenantId = tenantId;
            PaperNumber = paperNumber;
        }

        public int TenantId { get; private set; }
        public string PaperNumber { get; private set; }
    }
}
