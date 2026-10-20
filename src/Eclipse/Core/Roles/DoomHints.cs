using System;
using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Core;

internal enum DoomableType
{
    /// <summary>No special hint: the role is "not from this town". Every role that does not choose another kind.</summary>
    Default = 0,

    /// <summary>An altered perception of reality.</summary>
    Perception = 1,

    /// <summary>An insight for private information.</summary>
    Insight = 2,

    /// <summary>An unusual obsession with dead bodies.</summary>
    Death = 3,

    /// <summary>Well trained in hunting down prey.</summary>
    Hunter = 4,

    /// <summary>Spreads fear amongst the group.</summary>
    Fearmonger = 5,

    /// <summary>Hides to guard themselves or others.</summary>
    Protective = 6,

    /// <summary>Has a trick up their sleeve.</summary>
    Trickster = 7,

    /// <summary>Capable of relentless attacks.</summary>
    Relentless = 8,
}

internal static class DoomHints
{

    public static IReadOnlyList<T> RolesFor<T>(IEnumerable<(T Role, DoomableType Hint)> candidates, DoomableType hint, Func<T, string> name)
    {
        return candidates
            .Where(x => x.Hint == hint)
            .Select(x => x.Role)
            .OrderBy(name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }
}
