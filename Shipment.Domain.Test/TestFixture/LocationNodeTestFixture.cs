using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;

namespace Shipment.Domain.Test.TestFixture
{
    public class LocationNodeTestFixture
    {
        private readonly ScheduleTestFixture scheduleTestFixture;
        private readonly RouteTestFixture routeTestFixture;

        public LocationNodeTestFixture()
        {
            scheduleTestFixture = new ScheduleTestFixture();
            routeTestFixture = new RouteTestFixture();
        }

        public IRouteRepository RouteRepository => routeTestFixture.RouteRepository;
        public ITransportScheduleRepository ScheduleRepository => scheduleTestFixture.ScheduleRepository;
    }
}
