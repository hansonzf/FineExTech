using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;

namespace Shippment.Domain.Services
{
    public interface IPrepareScheduleService
    {
        bool AssignEquipmentToSchedule(TransportSchedule schedule, Equipment equipment);
    }
}
