using System.Linq;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class WardenFortifyButton : EclipsePlayerButton<WardenRole>
{
    public override string Name => "Eclipse.Button.WardenFortify";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 10f;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("WardenFortify", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Warden;

    private static bool IsFortifying =>
        Protections.Ledger.TargetsOf(ProtectionKind.Fortify, PlayerControl.LocalPlayer.PlayerId, Protections.Now).Any();

    public override bool CanUse()
    {
        return base.CanUse() && ProtectionRules.CanFortify(IsFortifying);
    }

    // Once the Warden is fortifying someone there is nobody to aim at: no target, no outline on the closest player.
    protected override bool CanTarget(WardenRole role)
    {
        return ProtectionRules.CanFortify(IsFortifying);
    }

    protected override void OnClick()
    {
        if (Target != null)
        {
            WardenRpcs.RpcWardenFortify(PlayerControl.LocalPlayer, Target);
        }
    }
}
