using DomainBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orderpool.Api.Models.OrderWatcherAggregate;
using Orderpool.Api.Models.RemoteOrderAggregate;

namespace Orderpool.Api.Infrastructure
{
    public class OrderpoolDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public OrderpoolDbContext(DbContextOptions<OrderpoolDbContext> options)
            : base(options)
        {
            
        }

        public OrderpoolDbContext(DbContextOptions<OrderpoolDbContext> options, IMediator mediator)
            : this(options)
        {
            _mediator = mediator;
        }

        public DbSet<OrderWatcher> OrderWatchers { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region configuration OrderWatcher Entity
            var orderWatcherBuilder = modelBuilder.Entity<OrderWatcher>();
            orderWatcherBuilder.HasKey(x => x.Id);
            orderWatcherBuilder.Property(x => x.Handler).IsConcurrencyToken();
            orderWatcherBuilder.Property(x => x.OrderUuid).IsRequired();
            orderWatcherBuilder.Property(x => x.OriginOrderPK).IsRequired();

            orderWatcherBuilder.HasIndex(x => x.OrderUuid).IsUnique();
            #endregion

            #region configuration OrderProcess Entity
            var orderProcessBuilder = modelBuilder.Entity<OrderProcess>();
            orderProcessBuilder.HasKey(x => x.Id).IsClustered(false);
            orderProcessBuilder.HasOne<OrderWatcher>().WithMany().HasForeignKey(x => x.WatcherId);
            orderProcessBuilder.Property(x => x.Result).HasMaxLength(8000);

            orderProcessBuilder.HasIndex(x => x.WatcherId).IsClustered(true);
            #endregion

            #region configuration RemoteOrder Entity
            var remoteOrderBuilder = modelBuilder.Entity<RemoteOrder>();
            remoteOrderBuilder.HasKey(x => x.Id);
            remoteOrderBuilder.Property(x => x.ContentOfOrder).IsRequired().HasColumnType("text");

            remoteOrderBuilder.HasIndex(x => x.OrderUuid).IsUnique();
            #endregion
        }
    }

    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderpoolDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
