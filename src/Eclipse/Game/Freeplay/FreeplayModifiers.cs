using System;
using System.Collections.Generic;
using System.Linq;
using Eclipse.Modifiers;
using MiraAPI.Modifiers;

namespace Eclipse.Game;

internal static class FreeplayModifiers
{
    private static readonly Type[] Catalog =
    [
        typeof(ButtonBarryModifier),
        typeof(DrunkModifier),
        typeof(FlashModifier),
        typeof(GiantModifier),
        typeof(MiniModifier),
        typeof(SixthSenseModifier),
        typeof(SleuthModifier),
        typeof(TiebreakerModifier),
    ];

    private static List<UniversalModifier>? _samples;

    public static IReadOnlyList<UniversalModifier> Samples => _samples ??= Catalog
        .Select(type => (UniversalModifier)Activator.CreateInstance(type)!)
        .OrderBy(modifier => modifier.ModifierName)
        .ToList();

    public static uint? TypeIdOf(UniversalModifier modifier)
    {
        return ModifierManager.GetModifierTypeId(modifier.GetType());
    }
}
