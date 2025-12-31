using System.Collections.Generic;
using Dierentuin.Models.Domain;

namespace Dierentuin.Services.Interfaces
{
    public interface IAnimalService
    {
        void Sunrise(Animal animal);
        void Sunset(Animal animal);
        string FeedingTime(Animal animal);
        IList<string> CheckConstraints(Animal animal);
    }
}
