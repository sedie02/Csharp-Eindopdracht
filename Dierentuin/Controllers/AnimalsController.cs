using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Services.Interfaces;


namespace Dierentuin.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly ZooDbContext _context;

        private readonly IAnimalService _animalService;

        public AnimalsController(ZooDbContext context, IAnimalService animalService)
        {
            _context = context;
            _animalService = animalService;
        }



        // GET: /Animals
        public async Task<IActionResult> Index()
        {
            var animals = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .ToListAsync();

            return View(animals);
        }

        // GET: /Animals/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [HttpPost]
        public async Task<IActionResult> Sunrise(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            _animalService.Sunrise(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }


        [HttpPost]
        public async Task<IActionResult> Sunset(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            _animalService.Sunset(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }


        [HttpPost]
        public async Task<IActionResult> Feeding(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            _animalService.FeedingTime(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }


        [HttpGet]
        public async Task<IActionResult> Constraints(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            var results = _animalService.CheckConstraints(animal);
            return View(results);
        }

        // GET: /Animals/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Enclosures = _context.Enclosures.ToList();

            return View();
        }

        // POST: /Animals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Enclosures = _context.Enclosures.ToList();
                return View(animal);
            }

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Animals/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Enclosures = _context.Enclosures.ToList();

            return View(animal);
        }

        // POST: /Animals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Enclosures = _context.Enclosures.ToList();
                return View(animal);
            }

            _context.Update(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
