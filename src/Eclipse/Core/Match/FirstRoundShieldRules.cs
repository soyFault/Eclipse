using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Core;

internal sealed class FirstDeadBook
{
    private readonly List<string> _names = [];

    public int Capacity { get; } = 4;

    public IReadOnlyList<string> Names => _names;

    public void Record(string name)
    {
        if (!string.IsNullOrEmpty(name) && _names.Count < Capacity && !_names.Contains(name))
        {
            _names.Add(name);
        }
    }

    public string? Recipient(IReadOnlyCollection<string> presentNames)
    {
        foreach (var name in _names)
        {
            if (presentNames.Contains(name))
            {
                return name;
            }
        }

        return null;
    }

    public void Clear()
    {
        _names.Clear();
    }
}

internal static class FirstRoundShieldRules
{
    public static bool Blocks(bool targetHoldsShield, byte attackerId, byte targetId, bool ignoresDefense, bool inMeetingOrExile)
    {
        return targetHoldsShield && attackerId != targetId && !ignoresDefense && !inMeetingOrExile;
    }

    public static bool ShouldGive(bool settingOn, bool startOfMatch, bool inFreeplay, bool hasRecipient)
    {
        return settingOn && startOfMatch && !inFreeplay && hasRecipient;
    }
}
