using Eclipse.Core;
using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class GiantModifier : BodyModifier
{
    public override string IdPart => "Giant";

    public override Color ModifierColor => new Color32(255, 179, 77, 255);

    protected override float SpeedMultiplier => OptionGroupSingleton<UniversalOptions>.Instance.GiantSpeed;

    protected override float Scale => UniversalRules.GiantScale;

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.GiantAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.GiantChance;
    }
}
