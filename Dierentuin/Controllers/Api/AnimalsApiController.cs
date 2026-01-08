using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;
using Dierentuin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers.Api
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsApiController : ControllerBase
    {
        private readonly ZooDbContext _context;
        private readonly IAnimalService _animalService;

        public AnimalsApiController(ZooDbContext context, IAnimalService animalService)
        {
            _context = context;
            _animalService = animalService;
        }

        // GET: api/animals
        // GET: api/animals?dietaryClass=&size=&activityPattern=&categoryId=&enclosureId=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals(
            DietaryClass? dietaryClass,
            Size? size,
            ActivityPattern? activityPattern,
            int? categoryId,
            int? enclosureId)
        {
            IQueryable<Animal> query = _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure);

            if (dietaryClass.HasValue)
            {
                query = query.Where(a => a.DietaryClass == dietaryClass);
            }

            if (size.HasValue)
            {
                query = query.Where(a => a.Size == size);
            }

            if (activityPattern.HasValue)
            {
                query = query.Where(a => a.ActivityPattern == activityPattern);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.Category != null && a.Category.Id == categoryId);
            }

            if (enclosureId.HasValue)
            {
                query = query.Where(a => a.Enclosure != null && a.Enclosure.Id == enclosureId);
            }

            return await query.ToListAsync();
        }


        // GET: api/animals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return animal;
        }

        // POST: api/animals
        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }

        // PUT: api/animals/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/animals/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/animals/{id}/sunrise
        [HttpPost("{id}/sunrise")]
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

            return Ok($"{animal.Name} sunrise action executed.");
        }


        // POST: api/animals/{id}/sunset
        [HttpPost("{id}/sunset")]
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

            return Ok($"{animal.Name} sunset action executed.");
        }


        // POST: api/animals/{id}/feeding
        [HttpPost("{id}/feeding")]
        public async Task<ActionResult<string>> FeedingTime(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            var result = _animalService.FeedingTime(animal);
            await _context.SaveChangesAsync();

            return Ok(result);
        }


        // GET: api/animals/{id}/constraints
        [HttpGet("{id}/constraints")]
        public async Task<ActionResult<IList<string>>> CheckConstraints(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            var results = _animalService.CheckConstraints(animal);
            return Ok(results);
        }

    }
}
