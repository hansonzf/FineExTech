using AutoMapper;
using Merchant.Api.Dtos;
using Merchants.Api.Infrastructure;
using Merchants.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Merchants.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PartnersController : ControllerBase
    {
        private readonly ILogger<PartnersController> _logger;
        private readonly MerchantContext _context;
        private readonly IMapper _mapper;

        public PartnersController(ILogger<PartnersController> logger, MerchantContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Partner), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Partner>> GetMerchantById(long id)
        {
            if (id <= 0)
                return BadRequest("Parameter 'id' must large than 0");

            var merchant = await _context.Set<Partner>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (merchant is null)
                return NoContent();
            else
                return merchant;
        }

        [HttpGet("/exist/tenant/{tenantId}/merchant/{code}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<bool>> CheckMerchantCodeExist(int tenantId, string code)
        {
            if (tenantId <= 0 || string.IsNullOrEmpty(code))
                return BadRequest("'tenantId' and 'code' both are required");

            return await _context.Set<Partner>().AnyAsync(p => p.TenantId == tenantId && p.MerchantCode == code);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateMerchant([FromBody] PartnerDto item)
        {
            if (item is null)
                return BadRequest("POST body is required");

            if (string.IsNullOrEmpty(item.MerchantCode))
                return BadRequest("'MerchantCode' is required");

            if (item.TenantId <= 0)
                return BadRequest("'TenantId is required'");

            if (_context.Set<Partner>().Any(i => i.MerchantCode == item.MerchantCode && i.TenantId == item.TenantId))
                return Conflict(new { RequestBody = item, Message = "The cargo owner code has already exist" });

            var merchant = _mapper.Map<Partner>(item);
            await _context.Set<Partner>().AddAsync(merchant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMerchantById), new { id = item.Id }, item);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMerchant(long id)
        {
            if (_context.Set<Partner>().Any(i => i.Id == id))
            {
                _context.Entry(Partner.CreateInstance(id)).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }
    }
}