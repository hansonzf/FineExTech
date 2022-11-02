using Shipment.Domain.Test.TestFixture;
using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.ItineraryAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;
using Shippment.Domain.Events;

namespace Shipment.Domain.Test
{
    public class LocationNodeOperationTest : IClassFixture<LocationNodeTestFixture>
    {
        private ITransportScheduleRepository _scheduleRepository;
        private IRouteRepository _routeRepository;
        private IEquipmentRepository _equipmentRepository;

        public LocationNodeOperationTest(LocationNodeTestFixture fixture)
        {
            _scheduleRepository = fixture.ScheduleRepository;
            _routeRepository = fixture.RouteRepository;
            _equipmentRepository = fixture.EquipmentRepository;
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

        [Fact]
        public void Worker_scan_the_tracking_number_and_loading_cargo()
        {
            string trackingNumber = "123456789";
            LocationDescription location = new LocationDescription(1, "武汉发网一仓", "武汉");
            
            var itinarery = new Itinerary(trackingNumber);
            var handing = new LoadHanding(location);
            itinarery.Log(handing);
        }
    }
}