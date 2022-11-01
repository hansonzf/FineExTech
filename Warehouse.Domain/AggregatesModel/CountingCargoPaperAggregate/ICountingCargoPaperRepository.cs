using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate
{
    public interface ICountingCargoPaperRepository : IRepository<CountingCargoPaper>
    {
        Task<long> SaveCountingCargoPaper(CountingCargoPaper countingCargoPaper);
    }
}
