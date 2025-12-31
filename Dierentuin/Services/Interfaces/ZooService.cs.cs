using System.Linq;
using Dierentuin.Models.Domain;
using Dierentuin.Services.Interfaces;

namespace Dierentuin.Services.Implementations
{
    public class ZooService : IZooService
    {
        public void Sunrise(Zoo zoo)
        {
            zoo.Sunrise();
        }

        public void Sunset(Zoo zoo)
        {
            zoo.Sunset();
        }

        public IList<string> FeedingTime(Zoo zoo)
        {
            return zoo.FeedingTime();
        }

        public IList<string> CheckConstraints(Zoo zoo)
        {
            return zoo.CheckConstraints();
        }

        public void AutoAssign(Zoo zoo, bool resetExistingEnclosures)
        {
            if (resetExistingEnclosures)
            {
                zoo.Enclosures.Clear();
            }

            foreach (var animal in zoo.Animals.Where(a => a.Enclosure == null))
            {
                var enclosure = zoo.Enclosures.FirstOrDefault(e =>
                    e.SecurityLevel >= animal.SecurityRequirement &&
                    e.Size >= animal.SpaceRequirement);

                if (enclosure == null)
                {
                    enclosure = new Enclosure
                    {
                        Name = $"AutoEnclosure-{zoo.Enclosures.Count + 1}",
                        SecurityLevel = animal.SecurityRequirement,
                        Size = animal.SpaceRequirement * 2
                    };

                    zoo.Enclosures.Add(enclosure);
                }

                enclosure.Animals.Add(animal);
                animal.Enclosure = enclosure;
            }
        }
    }
}
