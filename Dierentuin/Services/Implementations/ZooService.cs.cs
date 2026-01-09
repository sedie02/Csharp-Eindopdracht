using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;
using Dierentuin.Services.Interfaces;
using System.Linq;

namespace Dierentuin.Services.Implementations
{
    public class ZooService : IZooService
    {


        private readonly IEnclosureService _enclosureService;

        public ZooService(IEnclosureService enclosureService)
         

        {
            _enclosureService = enclosureService;
        }
        public void Sunrise(Zoo zoo)
        {
            zoo.CurrentPhase = ZooPhase.Day;

            foreach (var enclosure in zoo.Enclosures)
            {
                _enclosureService.Sunrise(enclosure);
            }
        }

        public void Sunset(Zoo zoo)
        {
            zoo.CurrentPhase = ZooPhase.Night;

            foreach (var enclosure in zoo.Enclosures)
            {
                _enclosureService.Sunset(enclosure);
            }
        }



        public IList<string> FeedingTime(Zoo zoo)
        {
            var results = new List<string>();

            foreach (var enclosure in zoo.Enclosures)
            {
                results.AddRange(_enclosureService.FeedingTime(enclosure));
            }

            return results;
        }


        public IList<string> CheckConstraints(Zoo zoo)
        {
            var results = new List<string>();

            foreach (var animal in zoo.Animals.Where(a => a.Enclosure == null))
            {
                results.Add($"{animal.Name} is not assigned to any enclosure.");
            }

            foreach (var enclosure in zoo.Enclosures)
            {
                results.AddRange(_enclosureService.CheckConstraints(enclosure));
            }

            return results;
        }


        public void AutoAssign(Zoo zoo, bool resetExistingEnclosures)
        {
            if (resetExistingEnclosures)
            {
                foreach (var enclosure in zoo.Enclosures)
                {
                    enclosure.Animals.Clear();
                }

                foreach (var animal in zoo.Animals)
                {
                    animal.Enclosure = null;
                }
            }

            foreach (var animal in zoo.Animals.Where(a => a.Enclosure == null))
            {
                var suitableEnclosure = zoo.Enclosures.FirstOrDefault(e =>
                    e.SecurityLevel >= animal.SecurityRequirement &&
                    e.Size - e.Animals.Sum(x => x.SpaceRequirement) >= animal.SpaceRequirement);

                if (suitableEnclosure == null)
                {
                    suitableEnclosure = new Enclosure
                    {
                        Name = $"AutoEnclosure-{zoo.Enclosures.Count + 1}",
                        SecurityLevel = animal.SecurityRequirement,
                        Size = animal.SpaceRequirement * 2,
                        Climate = Climate.Temperate,
                        HabitatType = HabitatType.Grassland
                    };

                    zoo.Enclosures.Add(suitableEnclosure);
                }

                suitableEnclosure.Animals.Add(animal);
                animal.Enclosure = suitableEnclosure;
            }
        }

    }
}
