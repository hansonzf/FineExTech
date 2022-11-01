using Dapper;
using DomainBase;
using Grpc.Net.Client.Balancer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Orderpool.Api.Models.OrderWatcherAggregate;
using Polly;

namespace Orderpool.Api.Infrastructure
{
    public class OrderWatcherRepository : IOrderWatcherRepository
    {
        private readonly OrderpoolDbContext _dbContext;
        private readonly string appInstanceId;
        private readonly string connectionString;

        public OrderWatcherRepository(OrderpoolDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            appInstanceId = configuration["AppInstanceId"];
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public Task<int> BulkInsert(List<OrderWatcher> orders)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderWatcher?> FetchNextOrderWatcherAsync()
        {
            //var watcher = await Policy<OrderWatcher?>.Handle<DbUpdateConcurrencyException>()
            //                        .RetryAsync(1)
            //                        .ExecuteAsync(FetchCore);

            //return watcher;
            return await FetchCore();
        }

        public async Task<OrderWatcher?> GetByIdAsync(long id)
        {
            using var connection = new SqlConnection(connectionString);
            var watcher = connection.QueryFirstOrDefault<OrderWatcher>(@"
SELECT * FROM OrderWatchers WHERE Id = @id
", new { id = id});
            var process = connection.Query<OrderProcess>(@"
SELECT * FROM OrderProcess WHERE WatcherId = @watcherId
", new { watcherId = id}).ToList();

            if (watcher is not null)
                watcher.Processses = process;

            return watcher;
        }

        public async Task<bool> SaveAsync(OrderWatcher watcher)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveProcessAsync(OrderWatcher watcher)
        {
            using var connection = new SqlConnection(connectionString);
            int affectRows = 0;
            foreach (var item in watcher.Processses)
            {
                if (item.Id != 0) 
                    continue;

                await connection.ExecuteAsync(@"
INSERT INTO OrderProcess (WatcherId, Result, ProcessTime, OrderWatcherId)
VALUES (@watcherId, @result, @processTime, @orderWatcherId)
", new { 
                    watcherId = item.WatcherId, 
                    result = item.Result, 
                    processTime = item.ProcessTime, 
                    orderWatcherId = item.WatcherId
                });
                affectRows++;
            }

            return affectRows == watcher.Processses.Count(p => p.Id == 0);
        }

        private async Task<OrderWatcher?> FetchCore()
        {
            var watcher = await _dbContext.OrderWatchers
                .Where(o => o.Handler == appInstanceId && o.Status == ProcessStatus.None)
                .FirstOrDefaultAsync();

            if (watcher is not null)
            {
                watcher.PrepareOrder(appInstanceId);
                await _dbContext.SaveChangesAsync();
            }

            return watcher;
        }
    }
}
