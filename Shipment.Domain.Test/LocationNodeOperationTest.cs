using Shipment.Domain.Test.TestFixture;
using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.ItineraryAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;

namespace Shipment.Domain.Test
{
    public class LocationNodeOperationTest : IClassFixture<LocationNodeDataFixture>
    {
        private ITransportScheduleRepository _scheduleRepository;

        public LocationNodeOperationTest(LocationNodeDataFixture fixture)
        {
            _scheduleRepository = fixture.ScheduleRepository;
        }

        [Fact]
        public async Task When_carrier_get_ready_to_departure_location()
        {
            LocationDescription departure = new LocationDescription(1, "�人");
            var schedule = await _scheduleRepository.GetScheduleByEquipmentAsync("��AM73Z7");

            bool result = schedule.Execute(departure);

            Assert.NotNull(schedule);
            Assert.True(result);
            Assert.Equal(ScheduleStatus.Executed, schedule.Status);
        }

        [Fact]
        public async Task When_carrier_get_ready_to_departure_but_with_wrong_departure_location()
        {
            LocationDescription departure = new LocationDescription(2, "�Ϸ�");
            var schedule = await _scheduleRepository.GetScheduleByEquipmentAsync("��AM73Z7");

            bool result = schedule.Execute(departure);

            Assert.NotNull(schedule);
            Assert.False(result);
        }

        [Fact]
        public async Task When_carrier_get_ready_to_departure_but_with_wrong_status_of_schedule()
        {
            LocationDescription departure = new LocationDescription(2, "�Ϸ�");
            var schedule = await _scheduleRepository.GetScheduleByEquipmentAsync("��A62FD1");

            Assert.Throws<InvalidOperationException>(() => {
                bool result = schedule.Execute(departure);
            });
        }

        [Fact]
        public async Task When_carrier_arrived_location_should_check_in()
        {
            LocationDescription destination = new LocationDescription(3, "�Ͼ�");
            var schedule = await _scheduleRepository.GetScheduleByEquipmentAsync("��A62FD1");

            bool result = schedule.CheckIn(destination);

            Assert.NotNull(schedule);
            Assert.True(result);
            Assert.Equal(ScheduleStatus.Completed, schedule.Status);
        }

        [Fact]
        public async Task When_carrier_arrived_location_check_in_with_wrong_location()
        {
            LocationDescription destination = new LocationDescription(4, "�Ϻ�");
            var schedule = await _scheduleRepository.GetScheduleByEquipmentAsync("��A62FD1");

            bool result = schedule.CheckIn(destination);

            Assert.NotNull(schedule);
            Assert.False(result);
        }

        [Fact]
        public async Task When_carrier_arrived_location_check_in_with_wrong_schedule()
        {
            LocationDescription destination = new LocationDescription(3, "�Ͼ�");
            var schedule = await _scheduleRepository.GetScheduleByEquipmentAsync("��AM73Z7");

            Assert.Throws<InvalidOperationException>(() => {
                bool result = schedule.CheckIn(destination);
            });
        }
    }
}