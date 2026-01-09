using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Dierentuin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers
{
    public class EnclosuresController : Controller
    {
        private readonly ZooDbContext _context;
        private readonly IEnclosureService _enclosureService;

        public EnclosuresController(
            ZooDbContext context,
            IEnclosureService enclosureService)
        {
            _context = context;
            _enclosureService = enclosureService;
        }

        // GET: /Enclosures
        public async Task<IActionResult> Index()
        {
            var enclosures = await _context.Enclosures
                .Include(e => e.Animals)
                .ToListAsync();

            return View(enclosures);
        }

        // GET: /Enclosures/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // POST: /Enclosures/Sunrise/5
        [HttpPost]
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

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /Enclosures/Sunset/5
        [HttpPost]
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

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /Enclosures/Feeding/5
        [HttpPost]
        public async Task<IActionResult> Feeding(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            _enclosureService.FeedingTime(enclosure);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: /Enclosures/Constraints/5
        [HttpGet]
        public async Task<IActionResult> Constraints(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            var results = _enclosureService.CheckConstraints(enclosure);
            return View(results);
        }
    }
}
