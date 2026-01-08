using System.Collections.Generic;
using Dierentuin.Models.Domain;
using Dierentuin.Services.Interfaces;

namespace Dierentuin.Services.Implementations
{
    public class AnimalService : IAnimalService
    {
        public void Sunrise(Animal animal)
        {
            animal.Sunrise();
        }

        public void Sunset(Animal animal)
        {
            animal.Sunset();
        }

        public string FeedingTime(Animal animal)
        {
            return animal.FeedingTime();
        }

        public IList<string> CheckConstraints(Animal animal)
        {
            var results = new List<string>();

            if (animal.Enclosure == null)
            {
                results.Add($"{animal.Name} has no enclosure assigned.");
                return results;
            }

            if (animal.SecurityRequirement > animal.Enclosure.SecurityLevel)
            {
                results.Add(
                    $"{animal.Name} requires {animal.SecurityRequirement} security, " +
                    $"but enclosure '{animal.Enclosure.Name}' has {animal.Enclosure.SecurityLevel}."
                );
            }

            return results;
        }

    }
}
