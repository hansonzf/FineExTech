using Microsoft.AspNetCore.Mvc;
using Orderpool.Api.Models;
using Orderpool.Api.Models.OrderWatcherAggregate;
using Orderpool.Api.Models.RemoteOrderAggregate;
using Orderpool.Api.Services;

namespace Orderpool.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly IOrderWatcherRepository _repository;
        private readonly IOrderCenterSerivce _orderCenterService;
        private readonly ImportOrderService _importOrderService;

        public WorkerController(
            IOrderWatcherRepository repository, 
            IOrderCenterSerivce orderCenterService, 
            ImportOrderService importOrderService)
        {
            _repository = repository;
            _orderCenterService = orderCenterService;
            _importOrderService = importOrderService;
        }

        [HttpGet("pull/{orderBeforeTime}")]
        public async Task<IActionResult> Pull(DateTime orderBeforeTime)
        {
            IEnumerable<RemoteOrder> orderFromOrderCenter = await _orderCenterService.PullOrder(orderBeforeTime);
            await _importOrderService.ImportOrder(orderFromOrderCenter.ToList());

            return Ok();
        }

        [HttpPost("process")]
        public async Task<IActionResult> Execute()
        {
            var watcher = await _repository.FetchNextOrderWatcherAsync();
            if (watcher is null)
                return NoContent();

            watcher.StartProcess();
            await _repository.UnitOfWork.SaveEntitiesAsync();

            return Ok();
        }
    }
}