using System.Collections.Generic;
using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class SleuthModifier : UniversalModifier
{
    private readonly HashSet<byte> _reported = [];

    public override string IdPart => "Sleuth";

    public override Color ModifierColor => new Color32(128, 128, 0, 255);

    public bool Knows(byte playerId)
    {
        return _reported.Contains(playerId);
    }

    public void Learn(byte playerId)
    {
        _reported.Add(playerId);
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.SleuthAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.SleuthChance;
    }
}
