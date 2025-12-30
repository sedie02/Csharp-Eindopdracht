using System.Collections.Generic;
using System.Linq;

namespace Dierentuin.Models.Domain
{
    public class Zoo
    {
        public ICollection<Animal> Animals { get; set; } = new List<Animal>();

        public ICollection<Enclosure> Enclosures { get; set; } = new List<Enclosure>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public void Sunrise()
        {
            foreach (var enclosure in Enclosures)
            {
                enclosure.Sunrise();
            }
        }

        public void Sunset()
        {
            foreach (var enclosure in Enclosures)
            {
                enclosure.Sunset();
            }
        }

        public IList<string> FeedingTime()
        {
            return Enclosures
                .SelectMany(e => e.FeedingTime())
                .ToList();
        }

        public IList<string> CheckConstraints()
        {
            var results = new List<string>();

            foreach (var enclosure in Enclosures)
            {
                results.AddRange(enclosure.CheckConstraints());
            }

            return results;
        }
    }
}
