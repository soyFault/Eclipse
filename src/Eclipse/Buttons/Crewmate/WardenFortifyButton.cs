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

    public override bool CanUse()
    {
        var fortifying = Protections.Ledger.TargetsOf(ProtectionKind.Fortify, PlayerControl.LocalPlayer.PlayerId, Protections.Now).Any();
        return base.CanUse() && ProtectionRules.CanFortify(fortifying);
    }

    protected override void OnClick()
    {
        if (Target != null)
        {
            WardenRpcs.RpcWardenFortify(PlayerControl.LocalPlayer, Target);
        }
    }
}
