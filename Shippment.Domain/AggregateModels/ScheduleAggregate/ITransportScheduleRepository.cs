using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ScheduleAggregate
{
    public interface ITransportScheduleRepository : IRepository<TransportSchedule>
    {
        Task<TransportSchedule> GetAsync(long id);
        Task<bool> DeleteAsync(long id);
        Task<TransportSchedule> GetScheduleByEquipmentAsync(string equipmentIdentifier);
        Task<TransportSchedule> GetScheduleByEquipmentAsync(long equipmentId);
        Task<bool> CreateNewScheduleAsync(TransportSchedule schedule);
    }
}
