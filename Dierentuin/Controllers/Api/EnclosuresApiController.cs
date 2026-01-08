using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;
using Dierentuin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers.Api
{
    [ApiController]
    [Route("api/enclosures")]
    public class EnclosuresApiController : ControllerBase
    {
        private readonly ZooDbContext _context;
        private readonly IEnclosureService _enclosureService;

        public EnclosuresApiController(ZooDbContext context, IEnclosureService enclosureService)
        {
            _context = context;
            _enclosureService = enclosureService;
        }

        // GET: api/enclosures?securityLevel=&climate=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosures(
            SecurityLevel? securityLevel,
            Climate? climate)
        {
            IQueryable<Enclosure> query = _context.Enclosures
                .Include(e => e.Animals);

            if (securityLevel.HasValue)
            {
                query = query.Where(e => e.SecurityLevel == securityLevel);
            }

            if (climate.HasValue)
            {
                query = query.Where(e => e.Climate == climate);
            }

            return await query.ToListAsync();
        }

        // GET: api/enclosures/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosure(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return enclosure;
        }

        // POST: api/enclosures
        [HttpPost]
        public async Task<ActionResult<Enclosure>> CreateEnclosure(Enclosure enclosure)
        {
            _context.Enclosures.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnclosure), new { id = enclosure.Id }, enclosure);
        }

        // PUT: api/enclosures/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnclosure(int id, Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return BadRequest();
            }

            _context.Entry(enclosure).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/enclosures/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(int id)
        {
            var enclosure = await _context.Enclosures.FindAsync(id);

            if (enclosure == null)
            {
                return NotFound();
            }

            _context.Enclosures.Remove(enclosure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/enclosures/{id}/sunrise
        [HttpPost("{id}/sunrise")]
        public async Task<IActionResult> Sunrise(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            _enclosureService.Sunrise(enclosure);
            await _context.SaveChangesAsync();

            return Ok($"Sunrise executed for enclosure '{enclosure.Name}'.");
        }

        // POST: api/enclosures/{id}/sunset
        [HttpPost("{id}/sunset")]
        public async Task<IActionResult> Sunset(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            _enclosureService.Sunset(enclosure);
            await _context.SaveChangesAsync();

            return Ok($"Sunset executed for enclosure '{enclosure.Name}'.");
        }

        // POST: api/enclosures/{id}/feeding
        [HttpPost("{id}/feeding")]
        public async Task<ActionResult<IList<string>>> FeedingTime(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            var results = _enclosureService.FeedingTime(enclosure);
            await _context.SaveChangesAsync();

            return Ok(results);
        }

        // GET: api/enclosures/{id}/constraints
        [HttpGet("{id}/constraints")]
        public async Task<ActionResult<IList<string>>> CheckConstraints(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            var results = _enclosureService.CheckConstraints(enclosure);
            return Ok(results);
        }
    }
}
