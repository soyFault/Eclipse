using Eclipse.Core;
using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class MiniModifier : BodyModifier
{
    public override string IdPart => "Mini";

    public override Color ModifierColor => new Color32(230, 230, 230, 255);

    protected override float SpeedMultiplier => OptionGroupSingleton<UniversalOptions>.Instance.MiniSpeed;

    protected override float Scale => UniversalRules.MiniScale;

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.MiniAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.MiniChance;
    }
}
