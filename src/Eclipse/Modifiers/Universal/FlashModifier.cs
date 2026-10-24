using Eclipse.Core;
using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class FlashModifier : BodyModifier
{
    public override string IdPart => "Flash";

    public override Color ModifierColor => new Color32(255, 140, 0, 255);

    protected override float SpeedMultiplier => OptionGroupSingleton<UniversalOptions>.Instance.FlashSpeed;

    protected override float Scale => UniversalRules.NormalScale;

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.FlashAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.FlashChance;
    }
}
