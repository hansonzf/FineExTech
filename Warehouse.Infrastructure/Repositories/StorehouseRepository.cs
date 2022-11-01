using DomainBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Infrastructure.Repositories
{
    public class StorehouseRepository : IStorehouseRepository
    {
        private readonly WarehouseContext _context;

        public StorehouseRepository(WarehouseContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<long> CreateStorehouseAsync(Storehouse storehouse)
        {
            if (storehouse is not null && storehouse.IsValid())
            {
                await _context.Set<Storehouse>().AddAsync(storehouse);
                await UnitOfWork.SaveEntitiesAsync();

                return storehouse.Id;
            }

            return 0;
        }

        public async Task<Storehouse?> GetStorehouseAsync(long id)
        {
            return await _context.Set<Storehouse>()
                .AsNoTracking()
                .Include(s => s.StockLocations)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SaveStorehouseChangesAsync(Storehouse storehouse)
        {
            if (storehouse is not null && storehouse.IsValid())
            {
                var entry = _context.Set<Storehouse>().Attach(storehouse);
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                try
                {
                    await UnitOfWork.SaveEntitiesAsync();
                    return true;
                }
                catch (Exception)
                {
                    // here will log something

                    return false;
                }                
            }

            return false;
        }
    }
}
