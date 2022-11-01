using Moq;
using Shipment.Domain.Test.MockAggregate;
using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;

namespace Shipment.Domain.Test.TestFixture
{
    public class LocationNodeDataFixture
    {
        private readonly List<Route> _routesTestData;
        private readonly List<TransportSchedule> _scheduleTestData;
        private Mock<IRouteRepository> _mockRouteRepository;
        private Mock<ITransportScheduleRepository> _mockScheduleRepository;

        public LocationNodeDataFixture()
        {
            _mockRouteRepository = new Mock<IRouteRepository>();
            _mockScheduleRepository = new Mock<ITransportScheduleRepository>();
            _routesTestData = RouteProxy.SeedTestData();
            _scheduleTestData = ScheduleProxy.SeedTestData();

            _mockRouteRepository.Setup(rp => rp.GetAsync(It.Is<long>(id => id > 0 && id <= 3)))
                .ReturnsAsync((long id) => _routesTestData.First(r => r.Id == id));


            _mockScheduleRepository.Setup(r => r.GetScheduleByEquipmentAsync(It.Is<string>(str => !string.IsNullOrEmpty(str))))
                .ReturnsAsync((string equipId) => _scheduleTestData.Where(s => s.Equipment.Identifier == equipId && s.Status == ScheduleStatus.Executed).FirstOrDefault());
        }

        public IRouteRepository RouteRepository => _mockRouteRepository.Object;
        public ITransportScheduleRepository ScheduleRepository => _mockScheduleRepository.Object;
    }
}
