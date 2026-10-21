using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class MirrorcasterUnleashButton : EclipsePlayerButton<MirrorcasterRole>, IKillButton
{
    public override string Name => "Eclipse.Button.MirrorcasterUnleash";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<MirrorcasterOptions>.Instance.UnleashCooldown, 5f, 120f);

    public override float InitialCooldown => 10f;

    public override int MaxUses => 0;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("MirrorcasterUnleash", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.PrimaryAbility;

    public override Color TextOutlineColor => EclipseColors.Mirrorcaster;

    public override bool IsTargetValid(PlayerControl? target)
    {
        if (LocalRole is not { } role)
        {
            return false;
        }

        var carriesOwnMirror = target != null && Mirrors.On(target.PlayerId) is { } mirror && mirror.Mirrorcaster == PlayerControl.LocalPlayer.PlayerId;
        return MirrorcasterRules.CanUnleashAt(role.UnleashesAvailable, carriesOwnMirror) && base.IsTargetValid(target);
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        PlayerControl.LocalPlayer.RpcCustomMurder(Target, MeetingCheck.OutsideMeeting);
        MirrorcasterRpcs.RpcMirrorUnleash(PlayerControl.LocalPlayer);
    }
}
