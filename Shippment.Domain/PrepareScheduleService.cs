using Shippment.Domain.AggregateModels;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.ScheduleAggregate;
using Shippment.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain
{
    public class PrepareScheduleService : IPrepareScheduleService
    {
        public bool AssignEquipmentToSchedule(TransportSchedule schedule, Equipment equipment)
        {
            if (schedule.Status != ScheduleStatus.Created || schedule.Status != ScheduleStatus.Standby)
                throw new InvalidOperationException("Only Created or Standby transport schedule could be assign equipment");

            throw new NotImplementedException();
        }
    }
}
