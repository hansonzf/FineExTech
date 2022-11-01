using Autofac;
using Autofac.Core;
using EventBus;
using EventBus.Interfaces;
using EventBus.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Warehouse.Api;
using Warehouse.Api.Application.IntegrationEvents.EventHandling;
using Warehouse.Api.Application.IntegrationEvents.Events;
using Warehouse.Domain.AggregatesModel.InventoryAggregate;
using Warehouse.Domain.AggregatesModel.StockInAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WarehouseContext>(options => {
    options.UseSqlServer(connectionString);
});

builder.AddEventBus();
builder.RegisterMediator();

builder.Services.AddScoped<IStorehouseRepository, StorehouseRepository>();
builder.Services.AddScoped<IStockInPaperRepository, StockInPaperRepository>();
builder.Services.AddScoped<IInventoryRecordRepository, InventoryRecordRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseEventBus();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }