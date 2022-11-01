using Microsoft.Data.SqlClient;
using TempConsoleApp;
using Z.Dapper.Plus;

var testWatcherData = OrderWatcher.FakerData.Generate(1000).ToList();
var testOrderData = RemoteOrder.FakerData.Generate(1000).ToList();

foreach (var item in testWatcherData)
{
    var order = testOrderData.FirstOrDefault(x => x.Id == item.Id);
    if (order is not null)
        item.OrderUuid = order.OrderUuid;
}

var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OrderpoolDb;Integrated Security=True;Connect Timeout=30");
var context = new DapperPlusContext(connection);
context.Entity<OrderWatcher>().Table("OrderWatchers").Identity(x => x.Id);
context.BulkInsert(testWatcherData);

var connection1 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OrderpoolDb;Integrated Security=True;Connect Timeout=30");
var context1 = new DapperPlusContext(connection);
context1.Entity<RemoteOrder>().Table("RemoteOrder").Identity(x => x.Id);
context1.BulkInsert(testOrderData);

Console.ReadLine();