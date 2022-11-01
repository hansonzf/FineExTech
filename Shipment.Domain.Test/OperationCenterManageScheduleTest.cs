using MediatR;
using Shipment.Domain.Test.MockAggregate;
using Shipment.Domain.Test.TestFixture;
using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using Shippment.Domain.Events;
using System.Net.NetworkInformation;

namespace Shipment.Domain.Test
{
    public class OperationCenterManageScheduleTest : IClassFixture<ScheduleDataFixture>
    {
        private IRouteRepository _routeRepository;
        private IEquipmentRepository _equipmentRepository;
        private ITransportScheduleRepository _scheduleRepository;
        private ITransportOrderRepository _orderRepository;

        public OperationCenterManageScheduleTest(ScheduleDataFixture fixture)
        {
            _routeRepository = fixture.RouteRepository;
            _equipmentRepository = fixture.EquipmentRepository;
            _scheduleRepository = fixture.ScheduleRepository;
            _orderRepository = fixture.TransportOrderRepository;
        }

        //[Fact]
        //public async void Make_transport_schedule_should_success()
        //{
        //    long routeId = 1; int legOfRoute = 0;
        //    var equipments = await _equipmentRepository.GetAvailableEquipmentAsync(1);
        //    var equipment = equipments.First();
        //    var route = await _routeRepository.GetAsync(routeId);
        //    var leg = route.GetRouteLeg(legOfRoute);

        //    var schedule = new TransportSchedule(leg, equipment.Description, DateTime.Now.AddHours(8));

        //    Assert.NotNull(schedule);
        //    Assert.Equal(1, schedule.From.LocationId);
        //    Assert.Equal(4, schedule.To.LocationId);
        //    Assert.Equal("鄂A62FD1", schedule.Equipment.Identifier);
        //    Assert.Equal(TransportMethod.ByRoad, schedule.Method);
        //    Assert.Equal(ScheduleStatus.Created, schedule.Status);
        //}

        [Fact]
        public void Generate_pickup_schedule_should_success()
        {
            PickupDescription pickupDesc = new PickupDescription(
                true,
                EquipmentType.Vehicle,
                "武汉市洪山区东港科技园2栋5楼",
                "张峰", "18571855277",
                new DateTime(2022, 10, 28, 18, 0, 0),
                "货大约5个方，需要携带起重设备");
            var equipment = new EquipmentDescription(1, "鄂A62FD1", EquipmentType.Vehicle);
            var setoutTime = new DateTime(2022, 10, 31, 9, 0, 0);
            var from = new LocationDescription(1, "发网武汉仓");

            PickupSchedule actualSchedule = new PickupSchedule(equipment, setoutTime, from, pickupDesc);                

            Assert.NotNull(actualSchedule);
            Assert.Equal(ScheduleStatus.Created, actualSchedule.Status);
            Assert.Equal(equipment, actualSchedule.Equipment);
            Assert.Equal(1, actualSchedule.From.LocationId);
            Assert.Equal(pickupDesc.DetailAddress, actualSchedule.To.LocationName);
            Assert.Equal(pickupDesc.ContactName, actualSchedule.PickupInfo.ContactName);
            Assert.Equal(pickupDesc.Phone, actualSchedule.PickupInfo.Phone);
            Assert.Equal(pickupDesc.PickupCode, actualSchedule.PickupInfo.PickupCode);
        }

        [Fact]
        public async Task Preparing_dispatch_schedule_will_generate_pickup()
        {
            var dispatching = await _scheduleRepository.GetAsync(7);

            dispatching.PrepareSchedule();
            var schedule = dispatching as PickupSchedule;

            Assert.NotNull(schedule);
            Assert.NotNull(schedule.PickupInfo.PickupCode);
        }

        [Fact]
        public async Task Executing_dispatch_schedule_at_correct_setout_place_should_success()
        {
            var schedule = await _scheduleRepository.GetAsync(9);
            LocationDescription setoutLocation = new LocationDescription(1, "武汉网点公司");

            bool result = schedule.Execute(setoutLocation);

            Assert.True(result);
        }

        [Fact]
        public async Task Executing_dispatch_schedule_at_wrong_setout_place_should_failed()
        {
            var schedule = await _scheduleRepository.GetAsync(10);
            LocationDescription setoutLocation = new LocationDescription(2, "错误的出发地");

            bool result = schedule.Execute(setoutLocation);

            Assert.False(result);
        }

        [Fact]
        public async Task After_executing_dispatch_schedule_should_publish_scheduleexecuted_event()
        {
            var schedule = await _scheduleRepository.GetAsync(10);
            LocationDescription setoutLocation = new LocationDescription(1, "武汉网点公司");

            bool result = schedule.Execute(setoutLocation);

            Assert.Contains(schedule.DomainEvents, e => e is ScheduleExecutedDomainEvent);
        }
    }
}
