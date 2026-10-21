using AmongUs.GameOptions;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class SonarTrackButton : EclipsePlayerButton<SonarRole>
{
    public override string Name => "Eclipse.Button.SonarTrack";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<SonarOptions>.Instance.TrackCooldown, 5f, 120f);

    public override float InitialCooldown => 10f;

    public override int MaxUses => (int)OptionGroupSingleton<SonarOptions>.Instance.MaxTracks;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("SonarTrack", new VanillaAbilityIcon(RoleTypes.Tracker, MiraAssets.CrewmateFile));

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Sonar;

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);
        LocalRole?.Tick(Time.deltaTime);
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        // Tracking someone twice would only waste a use.
        return base.IsTargetValid(target) && LocalRole is { } role && !role.IsTracking(target!);
    }

    protected override void OnClick()
    {
        if (Target == null || LocalRole is not { } role)
        {
            return;
        }

        role.StartTracking(Target);
        ResetTarget();
    }
}
