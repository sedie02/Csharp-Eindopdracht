using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;
using Dierentuin.Services.Implementations;
using Dierentuin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers
{
    public class ZooController : Controller
    {
        private readonly ZooDbContext _context;
        private readonly IZooService _zooService;
        private readonly ZooStateService _zooState;

        public ZooController(
            ZooDbContext context,
            IZooService zooService,
            ZooStateService zooState)
        {
            _context = context;
            _zooService = zooService;
            _zooState = zooState;
        }

        // GET: /Zoo
        public async Task<IActionResult> Index()
        {
            var zoo = await LoadZooAsync();

            ViewBag.CurrentPhase = _zooState.CurrentPhase.ToString();
            ViewBag.TotalAnimals = zoo.Animals.Count;
            ViewBag.TotalEnclosures = zoo.Enclosures.Count;

            return View(zoo);
        }




        // POST: /Zoo/Sunrise
        [HttpPost]
        public async Task<IActionResult> Sunrise()
        {
            _zooState.CurrentPhase = ZooPhase.Day;

            var zoo = await LoadZooAsync();
            _zooService.Sunrise(zoo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Sunset()
        {
            _zooState.CurrentPhase = ZooPhase.Night;

            var zoo = await LoadZooAsync();
            _zooService.Sunset(zoo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: /Zoo/Feeding
        [HttpPost]
        public async Task<IActionResult> Feeding()
        {
            var zoo = await LoadZooAsync();
            _zooService.FeedingTime(zoo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Zoo/AutoAssign
        [HttpPost]
        public async Task<IActionResult> AutoAssign(bool resetExistingEnclosures)
        {
            var zoo = await LoadZooAsync();
            _zooService.AutoAssign(zoo, resetExistingEnclosures);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Zoo/Constraints
        [HttpGet]
        public async Task<IActionResult> Constraints()
        {
            var zoo = await LoadZooAsync();
            var results = _zooService.CheckConstraints(zoo);

            return View(results);
        }

        // Helper
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
