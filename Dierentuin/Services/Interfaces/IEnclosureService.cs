using System.Collections.Generic;
using Dierentuin.Models.Domain;

namespace Dierentuin.Services.Interfaces
{
    public interface IEnclosureService
    {
        void Sunrise(Enclosure enclosure);
        void Sunset(Enclosure enclosure);
        IList<string> FeedingTime(Enclosure enclosure);
        IList<string> CheckConstraints(Enclosure enclosure);
    }
}
