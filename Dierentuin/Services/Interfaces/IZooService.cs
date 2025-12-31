using System.Collections.Generic;
using Dierentuin.Models.Domain;

namespace Dierentuin.Services.Interfaces
{
    public interface IZooService
    {
        void Sunrise(Zoo zoo);
        void Sunset(Zoo zoo);
        IList<string> FeedingTime(Zoo zoo);
        IList<string> CheckConstraints(Zoo zoo);

        void AutoAssign(Zoo zoo, bool resetExistingEnclosures);
    }
}
