using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Warehouse.Domain.AggregatesModel.StockInAggregate;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StockinPaperController : ControllerBase
    {
        private readonly ILogger<StockinPaperController> _logger;
        private readonly IStockInPaperRepository _repository;

        public StockinPaperController(ILogger<StockinPaperController> logger, IStockInPaperRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("/tenant/{tenantId}/owner/{cargoOwnerId}")]
        [ProducesResponseType(typeof(IEnumerable<StockInPaper>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<StockInPaper>>> GetStandbyPaper(int tenantId, long cargoOwnerId)
        {
            if (tenantId <= 0)
                return BadRequest("parameter tenantId must large than 0");
            if (cargoOwnerId <= 0)
                return BadRequest("parameter cargoOwnerId must large than 0");

            var result = await _repository.GetStandbyPaperByCargoOwner(tenantId, cargoOwnerId);
            if (result is null || !result.Any())
                return NotFound();

            return Ok(result);
        }
    }
}
