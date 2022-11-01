using DomainBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Infrastructure
{
    public class WarehouseContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public WarehouseContext(DbContextOptions<WarehouseContext> options)
            : base(options)
        { }

        public WarehouseContext(DbContextOptions<WarehouseContext> options, IMediator mediator)
            : this(options)
        {
            _mediator = mediator;
        }

        public DbSet<Storehouse> Storehouses { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
