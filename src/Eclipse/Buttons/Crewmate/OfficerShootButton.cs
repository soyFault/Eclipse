using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles;
using Eclipse.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class OfficerShootButton : EclipsePlayerButton<OfficerRole>, IKillButton
{
    public override string Name => "Eclipse.Button.OfficerShoot";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<OfficerOptions>.Instance.ShootCooldown, 5f, 120f);

    public override float InitialCooldown => 10f;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("OfficerShoot", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.PrimaryAbility;

    public override Color TextOutlineColor => EclipseColors.Officer;

    public override bool CanUse()
    {
        return base.CanUse() && LocalRole is { } officer && OfficerRules.CanShoot(officer.GunConfiscated, officer.LoadedBullets);
    }

    protected override bool IsRoundLocked => MatchRounds.IsFirstRoundLocked(OptionGroupSingleton<OfficerOptions>.Instance.FirstRoundShooting);

    protected override bool CanTarget(OfficerRole role)
    {
        return !IsRoundLocked;
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);
        if (Button != null && LocalRole is { } officer)
        {
            Button.usesRemainingText.gameObject.SetActive(true);
            Button.usesRemainingSprite.gameObject.SetActive(true);
            Button.usesRemainingText.text = $"{officer.LoadedBullets}/{(int)OptionGroupSingleton<OfficerOptions>.Instance.MaxBulletsAtOnce}";
        }
    }

    protected override void OnClick()
    {
        var shooter = PlayerControl.LocalPlayer;
        if (Target == null || Target.Data?.Role == null || shooter.Data.Role is not OfficerRole officer)
        {
            return;
        }

        var options = OptionGroupSingleton<OfficerOptions>.Instance;
        var (faction, alignmentId) = RoleClassification.Classify(Target.Data.Role);
        var shot = OfficerRules.Decide(faction, alignmentId, HasKilled(Target), options.ToShotSettings());

        // The bullet is spent either way; a misfire also confiscates the gun (the extra round counts the one that is under way).
        var rounds = shot == OfficerShot.Misfire ? (int)options.RoundsPunished + 1 : officer.RoundsBeforeReset;
        OfficerRpcs.RpcOfficerSync(shooter, rounds, officer.BulletsLeft, officer.LoadedBullets - 1);
        shooter.RpcCustomMurder(Target, MeetingCheck.OutsideMeeting);

        if (!options.CanSelfReport)
        {
            UnreportableBodies.Start(Target.PlayerId);
        }

        if (shot == OfficerShot.Misfire)
        {
            Helpers.CreateAndShowNotification($"<b>{MiraLocaleManager.Get("Eclipse.Role.Officer.BadKillFeedback")}</b>", Color.white);
            ScreenFlash.Flash(Color.red);
        }

        ResetTarget();
    }

    private static bool HasKilled(PlayerControl target)
    {
        return KillLogEvents.Log.HasKilled(target.PlayerId);
    }
}
