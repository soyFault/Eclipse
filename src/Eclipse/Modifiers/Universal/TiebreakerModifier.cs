using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class TiebreakerModifier : UniversalModifier
{
    public override string IdPart => "Tiebreaker";

    public override Color ModifierColor => new Color32(153, 230, 153, 255);

    // Only one player per match can break a tie, as in Town of Us Mira.
    public override int GetAmountPerGame()
    {
        return 1;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.TiebreakerChance;
    }
}
