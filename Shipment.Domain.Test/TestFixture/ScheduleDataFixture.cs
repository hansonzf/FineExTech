using Moq;
using Shipment.Domain.Test.MockAggregate;
using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using System.Net.NetworkInformation;

namespace Shipment.Domain.Test.TestFixture
{
    public class ScheduleDataFixture
    {
        private readonly List<Route> _routesTestData;
        private readonly List<TransportSchedule> _scheduleTestData;
        private readonly List<Equipment> _equipmentTestData;
        private readonly List<TransportOrder> _ordersTestData;
        private Mock<IRouteRepository> _mockRouteRepository;
        private Mock<IEquipmentRepository> _mockEquipmentRepository;
        private Mock<ITransportScheduleRepository> _mockTransportScheduleRepository;
        private Mock<ITransportOrderRepository> _mockOrderRepository;

        public ScheduleDataFixture()
        {
            _routesTestData = RouteProxy.SeedTestData();
            _scheduleTestData = ScheduleProxy.SeedTestData();
            _equipmentTestData = EquipmentProxy.SeedTestData();
            _ordersTestData = TransportOrderProxy.SeedTestData();

            FixRouteRepository();
            FixEquipmentRepository();
            FixTransOrderRepository();
            FixScheduleRepository();
        }

        private void FixRouteRepository()
        {
            _mockRouteRepository = new Mock<IRouteRepository>();
            _mockRouteRepository.Setup(rp => rp.GetAsync(It.Is<long>(id => id > 0 && id <= 3)))
                .ReturnsAsync((long id) => _routesTestData.First(r => r.Id == id));
        }

        private void FixEquipmentRepository()
        {
            _mockEquipmentRepository = new Mock<IEquipmentRepository>();
            //_mockEquipmentRepository.Setup(rp => rp.GetAvailableEquipmentAsync(It.Is<long>(locationId => locationId > 0)))
            //    .ReturnsAsync((long locationId) => _equipmentTestData.Where(r => r.CurrentLocation.LocationId == locationId));
        }

        private void FixTransOrderRepository()
        {
            _mockOrderRepository = new Mock<ITransportOrderRepository>();
            _mockOrderRepository.Setup(rp => rp.GetAsync(It.Is<long>(id => id > 0)))
                .ReturnsAsync((long id) => _ordersTestData.First(o => o.Id == id));
            _mockOrderRepository.Setup(rp => rp.GetWaitforAuditOrdersAsync())
                .ReturnsAsync(() => _ordersTestData.Where(o => o.Status == OrderStatus.Ordered).AsEnumerable());
        }

        private void FixScheduleRepository()
        {
            _mockTransportScheduleRepository = new Mock<ITransportScheduleRepository>();
            _mockTransportScheduleRepository.Setup(
                rp => rp.GetScheduleByEquipmentAsync(
                    It.Is<string>(identity => !string.IsNullOrEmpty(identity))))
                .ReturnsAsync(
                    (string identity) => _scheduleTestData.Where(
                        s => s.Equipment.Identifier == identity &&
                        s.Status == ScheduleStatus.Executed &&
                        s.Type == ScheduleType.Pickup).FirstOrDefault());
            _mockTransportScheduleRepository.Setup
                (rp => rp.GetAsync(It.Is<long>(id => id > 0 && id <= 10)))
                .ReturnsAsync(
                    (long id) => _scheduleTestData.FirstOrDefault(s => s.Id == id));
        }


        public IRouteRepository RouteRepository => _mockRouteRepository.Object;
        public IEquipmentRepository EquipmentRepository => _mockEquipmentRepository.Object;
        public ITransportScheduleRepository ScheduleRepository => _mockTransportScheduleRepository.Object;
        public ITransportOrderRepository TransportOrderRepository => _mockOrderRepository.Object;
    }
}
