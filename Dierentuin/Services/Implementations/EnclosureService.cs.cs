using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;
using Dierentuin.Services.Interfaces;
using System.Collections.Generic;

namespace Dierentuin.Services.Implementations
{
    public class EnclosureService : IEnclosureService
    {
        public void Sunrise(Enclosure enclosure)
        {
            foreach (var animal in enclosure.Animals)
            {
                animal.Sunrise();
            }
        }

        public void Sunset(Enclosure enclosure)
        {
            foreach (var animal in enclosure.Animals)
            {
                animal.Sunset();
            }
        }


        public IList<string> FeedingTime(Enclosure enclosure)
        {
            var results = new List<string>();
            var animalsToRemove = new List<Animal>();

            foreach (var predator in enclosure.Animals
                         .Where(a => a.DietaryClass == DietaryClass.Carnivore))
            {
                var prey = enclosure.Animals
                    .FirstOrDefault(a => predator.CanEat(a));

                if (prey != null)
                {
                    results.Add($"{predator.Name} eats {prey.Name}");
                    animalsToRemove.Add(prey);
                }
            }

            foreach (var prey in animalsToRemove)
            {
                enclosure.Animals.Remove(prey);
            }

            foreach (var animal in enclosure.Animals)
            {
                results.Add($"{animal.Name} eats its regular diet ({animal.DietaryClass})");
            }

            return results;
        }


        public IList<string> CheckConstraints(Enclosure enclosure)
        {
            var results = new List<string>();

            var totalRequiredSpace = enclosure.Animals.Sum(a => a.SpaceRequirement);
            if (totalRequiredSpace > enclosure.Size)
            {
                results.Add(
                    $"Enclosure '{enclosure.Name}' has insufficient space " +
                    $"({totalRequiredSpace}m² required, {enclosure.Size}m² available)."
                );
            }

            foreach (var animal in enclosure.Animals)
            {
                if (animal.SecurityRequirement > enclosure.SecurityLevel)
                {
                    results.Add(
                        $"{animal.Name} requires {animal.SecurityRequirement} security, " +
                        $"but enclosure '{enclosure.Name}' has {enclosure.SecurityLevel}."
                    );
                }
            }

            // Predator / prey conflicts
            foreach (var predator in enclosure.Animals
                         .Where(a => a.DietaryClass == DietaryClass.Carnivore))
            {
                foreach (var prey in enclosure.Animals)
                {
                    if (predator.CanEat(prey))
                    {
                        results.Add(
                            $"Conflict in enclosure '{enclosure.Name}': " +
                            $"{predator.Name} can eat {prey.Name}."
                        );
                    }
                }
            }

            return results;
        }

    }
}
