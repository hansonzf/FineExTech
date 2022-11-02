using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;

namespace Shipment.Domain.Test.TestFixture
{
    public class LocationNodeTestFixture
    {
        private readonly ScheduleTestFixture scheduleTestFixture;
        private readonly RouteTestFixture routeTestFixture;
        private readonly EquipmentTestFixture equipmentTestFixture;

        public LocationNodeTestFixture()
        {
            scheduleTestFixture = new ScheduleTestFixture();
            routeTestFixture = new RouteTestFixture();
            equipmentTestFixture = new EquipmentTestFixture();
        }

        public IRouteRepository RouteRepository => routeTestFixture.RouteRepository;
        public ITransportScheduleRepository ScheduleRepository => scheduleTestFixture.ScheduleRepository;
        public IEquipmentRepository EquipmentRepository => equipmentTestFixture.EquipmentRepository;
    }
}
