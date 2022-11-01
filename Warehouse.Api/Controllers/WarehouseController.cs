using Microsoft.AspNetCore.Mvc;
using System.Net;
using Warehouse.Api.Models;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IStorehouseRepository _repository;

        public WarehouseController(ILogger<WarehouseController> logger, IStorehouseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{storehouseId}", Name = "Get")]
        [ProducesResponseType(typeof(Storehouse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Storehouse>> GetById(long storehouseId)
        {
            if (storehouseId <= 0)
                return BadRequest("parameter 'storehouseId' must large than 0");

            var storehouse = await _repository.GetStorehouseAsync(storehouseId);
            if (storehouse is null)
                return NoContent();

            return Ok(storehouse);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Storehouse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateStorehouse(CreateStorehousePayload payload)
        {
            var storehouse = Storehouse.CreateInstance(payload.TenantId, payload.MerchantId, payload.StorehouseName);
            long newId =  await _repository.CreateStorehouseAsync(storehouse);

            return CreatedAtAction(nameof(GetById), new { storehouseId = newId }, storehouse);
        }

        [HttpPut("init")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InitializeWarehouse(InitWarehousePayload payload)
        {
            var storehouse = await _repository.GetStorehouseAsync(payload.StorehouseId);
            if (storehouse is null)
                return NoContent();

            if (!payload.StoreLocationsAllocation.Any())
                return BadRequest();

            storehouse.InitializeStorehouse(
                payload.StoreLocationsAllocation.Select(s => new StorehouseAllocation(s.Code, s.Name, s.Volume)).ToList(),
                Address.Empty());

            await _repository.SaveStorehouseChangesAsync(storehouse);
            return Ok();
        }

        [HttpPost("stockin/prepare/{storehouseId}/{paperNumber}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ReadyToReceiveCargo(long storehouseId, string paperNumber)
        {
            var storehouse = await _repository.GetStorehouseAsync(storehouseId);
            if (storehouse is null)
                return NoContent();

            storehouse.StockInProcess(paperNumber);
            _ = await _repository.SaveStorehouseChangesAsync(storehouse);

            return Ok();
        }

        [HttpPost("stockin/counting")]
        [ProducesResponseType(typeof(IEnumerable<StockArea>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<IEnumerable<StockArea>>> CountingCargo(CountingCargoPayload payload)
        {
            var storehouse = await _repository.GetStorehouseAsync(payload.StorehouseId);
            if (storehouse is null)
                return NoContent();

            var locations = storehouse.CountingWaitforStockCargo(
                payload.StockinPaperSerialNumber,
                payload.Results.Select(
                    r => new CountingCargoResult(
                        r.CargoOwner, 
                        r.SKU, 
                        r.Count, 
                        r.Volume)
                    ).ToList());
            await _repository.SaveStorehouseChangesAsync(storehouse);

            return Ok(locations);
        }

        [HttpPost("stockin/shelf")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ShelfCargo(ShelfCargoPayload payload)
        {
            var storehouse = await _repository.GetStorehouseAsync(payload.StorehouseId);
            if (storehouse is null)
                return NoContent();

            ShelfCargoResult result = new ShelfCargoResult
            {
                ShelfDetails = payload.ShelfedCargo.Select(s => new ShelfDetail(s.LocationCode, s.CargoOwner, s.SKU, s.Count, s.Volume)).ToList(),
                UnableStockCargoDetails = payload.UnableShelfedCargo.Select(s => new CountingCargoResult(s.CargoOwner, s.SKU, s.Count, s.Volume)).ToList()
            };
            storehouse.ShelfCargo(payload.StockinPaperSerialNumber, result);
            await _repository.SaveStorehouseChangesAsync(storehouse);

            return Ok();
        }
    }
}