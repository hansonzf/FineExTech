using LocationApi.Domain;
using LocationApi.Domain.AggregateModels.LocationAggregate;
using LocationApi.Payload;
using Microsoft.AspNetCore.Mvc;

namespace LocationApi.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _repository;
        private readonly ILogger _logger;

        public LocationController(ILocationRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/locations/1
        [HttpGet("{locationId}")]
        public async Task<ActionResult<Location>> GetLocation(long locationId)
        {
            if (locationId <= 0)
                return BadRequest("Parameter locationId must large than 0!");

            var location = await _repository.GetAsync(locationId);
            if (location is null)
                return NoContent();
            else
                return Ok(location);
        }

        // GET api/locations/ownedby/1
        [HttpGet("ownedby/{owner}")]
        public async Task<ActionResult<PaginatedItemsPayload<Location>>> GetLocations(long ownerId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            if (ownerId <= 0)
                return BadRequest("Parameter ownerId must large than 0!");

            var locations = await _repository.GetByOwnerAsync(ownerId, pageIndex, pageSize);
            var paylaod = new PaginatedItemsPayload<Location>
            { 
                Data = locations,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Result = true,
                Message = string.Empty
            };
            if (locations.Any())
                return Ok(paylaod);
            else
                return NoContent();
        }

        // POST api/locations
        [HttpPost]
        public async Task<ActionResult<Location>> CreateNewLocation([FromBody] CreateLocationPayload payload)
        {
            var ad = payload.Address;
            var address = new Address(ad.Country, ad.Province, ad.City, ad.Street, ad.PostalCode, ad.DetailAddress);
            var location = new Location(payload.OwnerId, payload.Code, payload.Name, address);

            try
            {
                var locationId = await _repository.CreateAsync(location);
                return CreatedAtAction(nameof(GetLocation), new { locationId = locationId }, location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}