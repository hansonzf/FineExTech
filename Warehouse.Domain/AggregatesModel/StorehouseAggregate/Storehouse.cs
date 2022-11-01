using DomainBase;
using System.Collections.ObjectModel;
using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;
using Warehouse.Domain.Events;

namespace Warehouse.Domain.AggregatesModel.StorehouseAggregate
{
    public class Storehouse : Entity, IAggregateRoot
    {
        protected HashSet<StockArea> _stockArea;
        public StorehouseInfo WarehouseInfo { get; protected set; }
        public Address? WarehouseAddress { get;  protected set;}
        public bool Initialized { get; protected set; }
        public IList<StockArea> StockAreaBoard 
        {
            get => _stockArea.ToList().AsReadOnly();
            protected set => _stockArea = value.ToHashSet();
        }

        public static Storehouse CreateInstance(int tenantId, string tenantName, long merchantId, string warehouseName)
        {
            var storehouse = new Storehouse();
            storehouse.WarehouseInfo = new StorehouseInfo(tenantId, tenantName, merchantId, warehouseName);
            storehouse.StockAreaBoard = new List<StockArea>();
            storehouse.WarehouseAddress = Address.Empty;

            return storehouse;
        }

        public void InitializeStorehouse(List<StorehouseAllocation> layout, Address address)
        {
            if (layout.Any())
            {
                layout.ForEach(l => {
                    AddLocation(l.Code, l.Name, l.AreaType, l.MaxCapability);
                });
            }

            WarehouseAddress = address;

            AddDomainEvent(new SetupStorehouseDomainEvent(WarehouseInfo.TenantId, WarehouseInfo.MerchantId, Id));
        }

        public IEnumerable<StockArea> FilterAreaByTag(params string[] tags)
        {
            if (tags is not null && tags.Any())
            {
                return _stockArea.Where(s => s.TagList.Intersect(tags).Count() > 0).AsEnumerable();
            }

            return new StockArea[0];
        }

        public IEnumerable<StockArea> FilterArea(StockAreaType areaType, float requiredLoad = 0)
        {
            IEnumerable<StockArea> result = _stockArea.Where(s => s.Useable && s.Type == areaType);

            if (requiredLoad == 0)
                result = result.Where(s => s.LeftLoad > 0).AsEnumerable();
            else
                result = result.Where(s => s.LeftLoad > requiredLoad).AsEnumerable();

            if (!result.Any())
                result = result.Where(s => s.AllowOverload).AsEnumerable();

            return result;
        }

        public bool AddLocation(string code, string name, StockAreaType type, float maxLoad)
        {
            if (_stockArea.Any(l => l.Code == code))
                return false;

            var location = new StockArea(this.Id, code, type, name, maxLoad);
            _stockArea.Add(location);

            return _stockArea.Any(l => l.Code == code);
        }

        public StockArea? GetStockArea(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            return _stockArea.FirstOrDefault(s => s.Code == code);
        }

        public void ProcessedStockIn(string paperSerialNumber)
        {
            if (!Initialized)
                throw new InvalidOperationException("Storehouse not ready");

            AddDomainEvent(new ReadyToReceiveCargoDomainEvent(paperSerialNumber, WarehouseInfo.TenantId));
        }

        public StockArea[] CountingWaitforStockCargo(string paperSerialNumber, List<CountingCargoResult> countingResult)
        {
            if (!Initialized)
                throw new InvalidOperationException("Storehouse not ready");

            AddDomainEvent(new CountingArrivedCargoDomainEvent(paperSerialNumber, TenantId, countingResult));
            var availableLocations = _stockArea.Where(l => l.Useable && (l.MaxVolume > l.UsedVolume)).ToArray();
            if (!availableLocations.Any())
                availableLocations = _stockArea.Where(l => l.AllowOverload).ToArray();

            return availableLocations;
        }

        public void ShelfCargo(string paperSerialNumber, ShelfCargoResult shelfResult)
        {
            if (!Initialized)
                throw new InvalidOperationException("Storehouse still not ready");

            EnsureDocumentSerialNumberVaild(paperSerialNumber);
            shelfResult.ShelfDetails.ForEach(s => {
                var location = StockLocations.FirstOrDefault(l => l.LocationCode == s.LocationCode);
                location.Shelf(s.Volume);
            });

            if (!shelfResult.EntirellyShelfed)
                AddDomainEvent(new StorehouseNotHaveEnoughCapabilityDomainEvent(shelfResult.UnableStockCargoDetails));

            AddDomainEvent(new ShelfCargoByStockInPaperDomainEvent(TenantId, paperSerialNumber, shelfResult));
        }
    }
}
