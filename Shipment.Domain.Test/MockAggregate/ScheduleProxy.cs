using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using System.Linq.Expressions;
using System.Reflection;

namespace Shipment.Domain.Test.MockAggregate
{
    public class ScheduleProxy : TransportSchedule
    {
        public ScheduleProxy(Leg routeLeg, EquipmentDescription equipment, DateTime estimateSetoutTime, float transportInterval = 0)
            : base(routeLeg, estimateSetoutTime, transportInterval)
        { }

        private static List<Route> routes = RouteProxy.SeedTestData();
        private static List<TransportOrder> orders = TransportOrderProxy.SeedTestData();

        public static List<TransportSchedule> SeedTestData()
        {
            DateTime setoutTime = new DateTime(2022, 10, 27, 18, 0, 0);
            EquipmentDescription equipment = new EquipmentDescription(1, "鄂A73MZ7", EquipmentType.Vehicle);
            EquipmentDescription equipment2 = new EquipmentDescription(1, "鄂A62FD1", EquipmentType.Vehicle);
            TimeManagement eff;
            List<TransportSchedule> schedules = new List<TransportSchedule>();

            var created_Schedule_1 = new ScheduleProxy(routes[1].GetRouteLeg(1), equipment, setoutTime);
            created_Schedule_1.Id = 1;
            schedules.Add(created_Schedule_1);

            var created_Schedule_2 = new ScheduleProxy(routes[1].GetRouteLeg(1), equipment2, setoutTime);
            created_Schedule_2.Id = 2;
            schedules.Add(created_Schedule_1);

            var standby_Schedule_1 = new ScheduleProxy(routes[1].GetRouteLeg(1), equipment, setoutTime);
            standby_Schedule_1.Id = 3;
            standby_Schedule_1.Status = ScheduleStatus.Standby;
            schedules.Add(standby_Schedule_1);

            var standby_Schedule_2 = new ScheduleProxy(routes[1].GetRouteLeg(1), equipment2, setoutTime);
            standby_Schedule_2.Id = 4;
            standby_Schedule_1.Status = ScheduleStatus.Standby;
            schedules.Add(standby_Schedule_2);

            var executed_Schedule_1 = new ScheduleProxy(routes[1].GetRouteLeg(1), equipment, setoutTime);
            executed_Schedule_1.Id = 5;
            executed_Schedule_1.Status = ScheduleStatus.Standby;
            eff = executed_Schedule_1.Efficiency;
            executed_Schedule_1.Status = ScheduleStatus.Executed;
            executed_Schedule_1.Efficiency = new TimeManagement(eff.EstimateSetoutTime, eff.EstimateTransportInterval, DateTime.Now);
            schedules.Add(executed_Schedule_1);

            var executed_Schedule_2 = new ScheduleProxy(routes[1].GetRouteLeg(1), equipment2, setoutTime);
            executed_Schedule_2.Id = 6;
            executed_Schedule_2.Status = ScheduleStatus.Standby;
            eff = executed_Schedule_2.Efficiency;
            executed_Schedule_2.Status = ScheduleStatus.Executed;
            executed_Schedule_2.Efficiency = new TimeManagement(eff.EstimateSetoutTime, eff.EstimateTransportInterval, DateTime.Now);
            schedules.Add(executed_Schedule_2);


            var dispatching_Created_Status_Schedule_1 = new PickupSchedule(equipment, setoutTime, orders[5].Goal.Origin, orders[5].PickupCargoInfo);
            dispatching_Created_Status_Schedule_1.SetProperty(s => s.Id, 7);
            schedules.Add(dispatching_Created_Status_Schedule_1);

            var dispatching_Created_Status_Schedule_2 = new PickupSchedule(equipment2, setoutTime, orders[5].Goal.Origin, orders[5].PickupCargoInfo);
            dispatching_Created_Status_Schedule_2.SetProperty(s => s.Id, 8);
            schedules.Add(dispatching_Created_Status_Schedule_2);

            var dispatching_Standby_Status_Schedule_1 = new PickupSchedule(equipment, setoutTime, orders[5].Goal.Origin, orders[5].PickupCargoInfo);
            dispatching_Standby_Status_Schedule_1.SetProperty(s => s.Id, 9);
            dispatching_Standby_Status_Schedule_1.PrepareSchedule();
            schedules.Add(dispatching_Standby_Status_Schedule_1);

            var dispatching_Standby_Status_Schedule_2 = new PickupSchedule(equipment2, setoutTime, orders[5].Goal.Origin, orders[5].PickupCargoInfo);
            dispatching_Standby_Status_Schedule_2.SetProperty(s => s.Id, 10);
            dispatching_Standby_Status_Schedule_2.PrepareSchedule();
            schedules.Add(dispatching_Standby_Status_Schedule_2);

            return schedules;
        }
    }

    public static class PropertyExtension
    {
        public static void SetProperty<T, TValue>(this T target, Expression<Func<T, TValue>> memberLambda, TValue value)
        {
            if (memberLambda.Body is MemberExpression memberSelectorExpression)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value);
                }
            }
        }

        public static TValue GetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLambda, TValue @default)
        {
            if (memberLambda.Body is MemberExpression memberSelectorExpression)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    return (TValue)property.GetValue(target);
                }
            }
            return @default;
        }
    }
}
