using System;

namespace Dierentuin.Models.Enums
{
    [Flags]
    public enum HabitatType
    {
        None = 0,
        Forest = 1,
        Aquatic = 2,
        Desert = 4,
        Grassland = 8
    }
}
