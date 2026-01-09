using Dierentuin.Models.Domain;
using Dierentuin.Models.Enums;

namespace Dierentuin.Services.Implementations
{
    public class ZooStateService
    {
        public ZooPhase CurrentPhase { get; set; } = ZooPhase.Day;
    }
}
