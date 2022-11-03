using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Shipment.Domain.Test.TestFixture;
using Shippment.Domain;
using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.ItineraryAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using Shippment.Domain.Events;
using Xunit.Abstractions;

namespace Shipment.Domain.Test
{
    public class LocationNodeOperationTest : IClassFixture<LocationNodeTestFixture>
    {
        private readonly ITestOutputHelper _output;
        private ITransportScheduleRepository _scheduleRepository;
        private IRouteRepository _routeRepository;
        private IEquipmentRepository _equipmentRepository;
        private LocationNodeTestFixture _fixture;

        public LocationNodeOperationTest(LocationNodeTestFixture fixture, ITestOutputHelper output)
        {
            _scheduleRepository = fixture.ScheduleRepository;
            _routeRepository = fixture.RouteRepository;
            _equipmentRepository = fixture.EquipmentRepository;
            _fixture = fixture;
            _output = output;
        }

        private async Task<TransportSchedule> GenerateScheduleWithStandbyStatus()
        {
            long routeId = 1; int legOfRoute = 0;
            DateTime setoutTime = new DateTime(2022, 11, 11, 12, 0, 0);
            var route = await _routeRepository.GetAsync(routeId);
            var equipments = await _equipmentRepository.GetAvailableEquipmentAsync(1);
            var mycar = equipments.First();
            var leg = route.GetRouteLeg(legOfRoute);
            var schedule = new TransportSchedule(leg, setoutTime, 10);
            schedule.DispatchingEquipment(mycar.Description);

            return schedule;
        }

        [Fact]
        public async Task Executing_schedule_from_correct_setout_location_should_success()
        {
            LocationDescription expectOrigin = new LocationDescription(1, "武汉");
            // 该测试数据的始发地是武汉，目的地是上海
            var schedule = await GenerateScheduleWithStandbyStatus();

            bool result = schedule.Execute(expectOrigin);

            Assert.True(result);
            Assert.NotNull(schedule.Efficiency.FactSetoutTime);
            Assert.Equal(ScheduleStatus.Executed, schedule.Status);
            Assert.Contains(schedule.DomainEvents, e => e is ScheduleExecutedDomainEvent);
        }

        [Fact]
        public async Task Executing_schedule_from_wrong_departure_should_failed()
        {
            LocationDescription wrongDeparture = new LocationDescription(4, "上海");
            // 该测试数据的始发地是武汉，目的地是上海
            var schedule = await GenerateScheduleWithStandbyStatus();

            bool result = schedule.Execute(wrongDeparture);

            Assert.False(result);
            Assert.Null(schedule.Efficiency.FactSetoutTime);
            Assert.Equal(ScheduleStatus.Standby, schedule.Status);
        }

        [Fact]
        public async Task When_carrier_get_ready_to_departure_but_with_wrong_status_of_schedule()
        {
            LocationDescription departure = new LocationDescription(2, "合肥");
            var schedule = await _scheduleRepository.GetTransportScheduleByEquipmentAsync("鄂A62FD1");

            Assert.Throws<InvalidOperationException>(() => {
                bool result = schedule.Execute(departure);
            });
        }

        [Fact]
        public async Task When_carrier_arrived_location_should_check_in()
        {
            LocationDescription destination = new LocationDescription(3, "南京");
            var schedule = await _scheduleRepository.GetTransportScheduleByEquipmentAsync("鄂A62FD1");

            bool result = schedule.CheckIn(destination);

            Assert.NotNull(schedule);
            Assert.True(result);
            Assert.Equal(ScheduleStatus.Completed, schedule.Status);
        }

        [Fact]
        public async Task When_carrier_arrived_location_check_in_with_wrong_location()
        {
            LocationDescription destination = new LocationDescription(4, "上海");
            var schedule = await _scheduleRepository.GetTransportScheduleByEquipmentAsync("鄂A62FD1");

            bool result = schedule.CheckIn(destination);

            Assert.NotNull(schedule);
            Assert.False(result);
        }

        private Itinerary GenerateTestItinerary()
        {
            string trackingNumber = "TRS-1-00001";
            var route = GenerateTestRoute();
            var itinerary = new Itinerary(trackingNumber);
            itinerary.TrackRoute(route.Legs);

            return itinerary;
        }

        public Route GenerateTestRoute()
        {
            LocationDescription origin = new LocationDescription(1, "发网武汉临空港一仓", "武汉");
            LocationDescription destination = new LocationDescription(2, "发网总公司", "上海");
            LocationDescription step1 = new LocationDescription(3, "发网南京中转站", "南京");

            Segment[] segments = new Segment[2]
            {
                new Segment(origin, step1, 500),
                new Segment(step1, destination, 400)
            };

            return new Route("测试路由", origin, destination, segments);
        }

        [Fact]
        public void Worker_scan_tracking_number_and_loading_cargo()
        {
            string trackingNumber = "TRS-1-00001";
            LocationDescription wh = new LocationDescription(1, "发网武汉临空港一仓", "武汉");
            var itinerary = GenerateTestItinerary();

            var handing = new LoadHanding(wh);
            handing.Process(trackingNumber);
            itinerary.Log(handing);
            _output.WriteLine(itinerary.FlushLog());

            Assert.NotEmpty(itinerary.Handings);
        }

        [Fact]
        public void Worker_scan_tracking_number_and_then_depart()
        {
            string trackingNumber = "TRS-1-00001";
            LocationDescription wh = new LocationDescription(1, "发网武汉临空港一仓", "武汉");
            var itinerary = GenerateTestItinerary();

            var handing1 = new LoadHanding(wh);
            handing1.Process(trackingNumber);
            var handing2 = new DepartureHanding(wh);
            handing2.Process(trackingNumber);
            itinerary.Log(handing1);
            itinerary.Log(handing2);
            _output.WriteLine(itinerary.FlushLog());

            Assert.NotEmpty(itinerary.Handings);
        }
    }
}