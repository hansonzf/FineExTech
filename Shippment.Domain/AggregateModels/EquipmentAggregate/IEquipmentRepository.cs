using DomainBase;

namespace Shippment.Domain.AggregateModels.EquipmentAggregate
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        Task<Equipment> GetAsync(long id);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync(long locationId, DateTime? requireTime = null);
    }
}
