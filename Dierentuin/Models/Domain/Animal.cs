using System.Collections.Generic;
using Dierentuin.Models.Enums;

namespace Dierentuin.Models.Domain
{
    public class Animal
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public Category? Category { get; set; }

        public Size Size { get; set; }

        public DietaryClass DietaryClass { get; set; }

        public ActivityPattern ActivityPattern { get; set; }

        public ICollection<string> Prey { get; set; } = new List<string>();

        public Enclosure? Enclosure { get; set; }

        public double SpaceRequirement { get; set; }

        public SecurityLevel SecurityRequirement { get; set; }

        public bool IsAwake { get; private set; }

        public void Sunrise()
        {
            IsAwake = ActivityPattern switch
            {
                ActivityPattern.Diurnal => true,
                ActivityPattern.Nocturnal => false,
                ActivityPattern.Cathemeral => true,
                _ => IsAwake
            };
        }

        public void Sunset()
        {
            IsAwake = ActivityPattern switch
            {
                ActivityPattern.Diurnal => false,
                ActivityPattern.Nocturnal => true,
                ActivityPattern.Cathemeral => true,
                _ => IsAwake
            };
        }

        public string FeedingTime()
        {
            return DietaryClass switch
            {
                DietaryClass.Carnivore => "Eats meat",
                DietaryClass.Herbivore => "Eats plants",
                DietaryClass.Omnivore => "Eats plants and meat",
                DietaryClass.Insectivore => "Eats insects",
                DietaryClass.Piscivore => "Eats fish",
                _ => "Unknown diet"
            };
        }

        public IList<string> CheckConstraints()
        {
            var results = new List<string>();

            if (Enclosure == null)
            {
                results.Add("Animal is not assigned to an enclosure.");
            }

            if (Enclosure != null && SecurityRequirement > Enclosure.SecurityLevel)
            {
                results.Add("Enclosure security level is insufficient.");
            }

            return results;
        }
    }
}
