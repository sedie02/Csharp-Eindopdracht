using System.Collections.Generic;
using System.Linq;
using Dierentuin.Models.Enums;

namespace Dierentuin.Models.Domain
{
    public class Enclosure
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Animal> Animals { get; set; } = new List<Animal>();

        public Climate Climate { get; set; }

        public HabitatType HabitatType { get; set; }

        public SecurityLevel SecurityLevel { get; set; }

        public double Size { get; set; }

        public void Sunrise()
        {
            foreach (var animal in Animals)
            {
                animal.Sunrise();
            }
        }

        public void Sunset()
        {
            foreach (var animal in Animals)
            {
                animal.Sunset();
            }
        }

        public IList<string> FeedingTime()
        {
            var results = new List<string>();

            foreach (var animal in Animals)
            {
                results.Add($"{animal.Name}: {animal.FeedingTime()}");
            }

            return results;
        }

        public IList<string> CheckConstraints()
        {
            var results = new List<string>();

            var requiredSpace = Animals.Sum(a => a.SpaceRequirement);
            if (requiredSpace > Size)
            {
                results.Add("Enclosure does not have enough space for all animals.");
            }

            foreach (var animal in Animals)
            {
                results.AddRange(animal.CheckConstraints());
            }

            return results;
        }
    }
}
