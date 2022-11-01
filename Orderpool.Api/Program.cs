using MediatR;
using Microsoft.EntityFrameworkCore;
using Orderpool.Api.Application.CollaborateServices;
using Orderpool.Api.Infrastructure;
using Orderpool.Api.Models;
using Orderpool.Api.Models.OrderWatcherAggregate;
using Orderpool.Api.Models.RemoteOrderAggregate;
using Orderpool.Api.OrderProcessPipeline;
using Orderpool.Api.Services;
using System.Net.NetworkInformation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OrderpoolDbContext>(options => {
    options.UseSqlServer(connectionString);
});

Assembly[] assemblies = new Assembly[1]
{
    Assembly.GetExecutingAssembly()
};
builder.Services.AddMediatR(assemblies);
builder.Services.AddTransient<IPipelineBehavior<ProcessParameter, OrderWatcher>, CheckInventoryHandler>();
builder.Services.AddTransient<IPipelineBehavior<ProcessParameter, OrderWatcher>, FindOnlyWarehouseToDeliveryOrderHandler>();
builder.Services.AddTransient<IRequestHandler<ProcessParameter, OrderWatcher>, CompleteOrderProcessHandler>();


builder.Services.AddScoped<IOrderWatcherRepository, OrderWatcherRepository>();
builder.Services.AddScoped<IRemoteOrderRepository, RemoteOrderRepository>();
builder.Services.AddTransient<IOrderCenterSerivce, OrderCenterService>();
builder.Services.AddTransient<IOrderCenterHttpAdapter, OrderCenterHttpAdapter>();
builder.Services.AddTransient<ImportOrderService, ImportOrderService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
