using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class ButtonBarryModifier : UniversalModifier
{
    public override string IdPart => "ButtonBarry";

    public override Color ModifierColor => new Color32(204, 0, 204, 255);

    public override int GetAmountPerGame()
    {
        return 1;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.ButtonBarryChance;
    }
}
