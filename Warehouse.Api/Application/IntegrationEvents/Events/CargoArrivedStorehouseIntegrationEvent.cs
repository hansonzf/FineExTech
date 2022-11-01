using EventBus.Events;

namespace Warehouse.Api.Application.IntegrationEvents.Events
{
    public record CargoArrivedStorehouseIntegrationEvent : IntegrationEvent
    {
        public int TenantId { get; init; }
        public int WarehouseId { get; init; }
        public int CargoOwnerId { get; init; }
        public string CargoOwnerName { get; init; }
        public string Description { get; init; }
        /// <summary>
        /// sku and corresponding count number
        /// </summary>
        public Dictionary<string, int> CargoSummary { get; init; }
    }
}
