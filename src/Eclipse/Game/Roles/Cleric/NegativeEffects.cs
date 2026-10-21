using System;
using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Game;

internal sealed record NegativeEffect(string NameKey, Func<PlayerControl, bool> IsOn, Action<PlayerControl> Remove);

internal static class NegativeEffects
{
    private static readonly List<NegativeEffect> All = [];

    public static void Register(NegativeEffect effect)
    {
        if (All.All(e => e.NameKey != effect.NameKey))
        {
            All.Add(effect);
        }
    }

    public static List<NegativeEffect> On(PlayerControl player)
    {
        return All.Where(e => e.IsOn(player)).ToList();
    }
}
