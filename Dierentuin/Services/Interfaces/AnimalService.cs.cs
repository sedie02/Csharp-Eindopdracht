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
            return animal.CheckConstraints();
        }
    }
}
