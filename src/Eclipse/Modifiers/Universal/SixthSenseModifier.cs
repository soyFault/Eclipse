using Eclipse.Options.Host;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class SixthSenseModifier : UniversalModifier
{
    public override string IdPart => "SixthSense";

    public override Color ModifierColor => new Color32(0, 255, 153, 255);

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.SixthSenseAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<UniversalOptions>.Instance.SixthSenseChance;
    }

    // The Aurial already feels attacks and would get the same warning twice.
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role is not AurialRole;
    }
}
