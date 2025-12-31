using System.Collections.Generic;
using Dierentuin.Models.Domain;
using Dierentuin.Services.Interfaces;

namespace Dierentuin.Services.Implementations
{
    public class EnclosureService : IEnclosureService
    {
        public void Sunrise(Enclosure enclosure)
        {
            enclosure.Sunrise();
        }

        public void Sunset(Enclosure enclosure)
        {
            enclosure.Sunset();
        }

        public IList<string> FeedingTime(Enclosure enclosure)
        {
            return enclosure.FeedingTime();
        }

        public IList<string> CheckConstraints(Enclosure enclosure)
        {
            return enclosure.CheckConstraints();
        }
    }
}
