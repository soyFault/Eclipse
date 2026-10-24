using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class DrunkModifier : UniversalModifier
{
    public override string IdPart => "Drunk";

    public override Color ModifierColor => new Color32(119, 136, 0, 255);

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.DrunkAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.DrunkChance;
    }
}
