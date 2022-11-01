using MediatR;

namespace Warehouse.Api.Application.Commands
{
    public class ReceiveCargoCommand : IRequest<bool>
    {
        public ReceiveCargoCommand()
        {
            CargoList = new List<CargoItem>();
        }

        public ReceiveCargoCommand(long warehouseId, string warehouseName, long cargoOwnerId, string cargoOwnerName)
            : this()
        {
            WarehouseId = warehouseId;
            WarehouseName = warehouseName;
            CargoOwnerId = cargoOwnerId;
            CargoOwnerName = cargoOwnerName;
        }

        public long WarehouseId { get; private set; }
        public string WarehouseName { get; private set; }
        public long CargoOwnerId { get; private set; }
        public string CargoOwnerName { get; private set; }
        public List<CargoItem> CargoList { get; private set; }
    }

    public record CargoItem
    {
        public string SKU { get; init; }
        public int Count { get; init; }
        public string Name { get; init; }
    }
}
