using Bogus;
using Dierentuin.Data.Context;
using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Dierentuin.Data.Seed
{
    public static class ZooSeeder
    {
        public static void Seed(ZooDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Animals.Any())
            {
                return;
            }

            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);


            var categories = categoryFaker.Generate(5);
            context.Categories.AddRange(categories);

            var enclosureFaker = new Faker<Enclosure>()
                .RuleFor(e => e.Name, f => $"Enclosure {f.Random.Number(1, 100)}")
                .RuleFor(e => e.Size, f => f.Random.Double(50, 300))
                .RuleFor(e => e.SecurityLevel, f => f.PickRandom<SecurityLevel>())
                .RuleFor(e => e.Climate, f => f.PickRandom<Climate>())
                .RuleFor(e => e.HabitatType, f => f.PickRandom<HabitatType>());

            var enclosures = enclosureFaker.Generate(3);
            context.Enclosures.AddRange(enclosures);

            var animalFaker = new Faker<Animal>()
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Species, f => f.Random.Word())
                .RuleFor(a => a.Size, f => f.PickRandom<Size>())
                .RuleFor(a => a.DietaryClass, f => f.PickRandom<DietaryClass>())
                .RuleFor(a => a.ActivityPattern, f => f.PickRandom<ActivityPattern>())
                .RuleFor(a => a.SpaceRequirement, f => f.Random.Double(5, 50))
                .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<SecurityLevel>())
                .RuleFor(a => a.Category, f => f.PickRandom(categories))
                .RuleFor(a => a.Enclosure, f => f.PickRandom(enclosures));

            var animals = animalFaker.Generate(10);

            foreach (var animal in animals)
            {
                animal.Enclosure?.Animals.Add(animal);
                animal.Category?.Animals.Add(animal);
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();
        }
    }
}
