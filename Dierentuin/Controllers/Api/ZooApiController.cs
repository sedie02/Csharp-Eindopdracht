using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Dierentuin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers.Api
{
    [ApiController]
    [Route("api/zoo")]
    public class ZooApiController : ControllerBase
    {
        private readonly ZooDbContext _context;
        private readonly IZooService _zooService;

        public ZooApiController(ZooDbContext context, IZooService zooService)
        {
            _context = context;
            _zooService = zooService;
        }

        // POST: api/zoo/sunrise
        [HttpPost("sunrise")]
        public async Task<IActionResult> Sunrise()
        {
            var zoo = await LoadZooAsync();

            _zooService.Sunrise(zoo);
            await _context.SaveChangesAsync();

            return Ok("Sunrise executed for the entire zoo.");
        }

        // POST: api/zoo/sunset
        [HttpPost("sunset")]
        public async Task<IActionResult> Sunset()
        {
            var zoo = await LoadZooAsync();

            _zooService.Sunset(zoo);
            await _context.SaveChangesAsync();

            return Ok("Sunset executed for the entire zoo.");
        }

        // POST: api/zoo/feeding
        [HttpPost("feeding")]
        public async Task<ActionResult<IList<string>>> FeedingTime()
        {
            var zoo = await LoadZooAsync();

            var results = _zooService.FeedingTime(zoo);
            await _context.SaveChangesAsync();

            return Ok(results);
        }

        // GET: api/zoo/constraints
        [HttpGet("constraints")]
        public async Task<ActionResult<IList<string>>> CheckConstraints()
        {
            var zoo = await LoadZooAsync();

            var results = _zooService.CheckConstraints(zoo);
            return Ok(results);
        }

        // POST: api/zoo/autoassign?resetExistingEnclosures=true
        [HttpPost("autoassign")]
        public async Task<IActionResult> AutoAssign([FromQuery] bool resetExistingEnclosures = false)
        {
            var zoo = await LoadZooAsync();

            _zooService.AutoAssign(zoo, resetExistingEnclosures);
            await _context.SaveChangesAsync();

            return Ok(
                resetExistingEnclosures
                    ? "Zoo auto-assigned with full reset."
                    : "Zoo auto-assigned using existing enclosures."
            );
        }

        // Helper: load full zoo context
        private async Task<Zoo> LoadZooAsync()
        {
            var animals = await _context.Animals
                .Include(a => a.Enclosure)
                .ToListAsync();

            var enclosures = await _context.Enclosures
                .Include(e => e.Animals)
                .ToListAsync();

            return new Zoo
            {
                Animals = animals,
                Enclosures = enclosures
            };
        }
    }
}
