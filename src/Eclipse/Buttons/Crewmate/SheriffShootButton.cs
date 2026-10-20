using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class SheriffShootButton : EclipsePlayerButton<SheriffRole>, IKillButton
{

    public override string Name => "Eclipse.Button.SheriffShoot";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<SheriffOptions>.Instance.KillCooldown, 5f, 120f);

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("SheriffShoot", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.PrimaryAbility;

    public override Color TextOutlineColor => EclipseColors.Sheriff;

    public override float InitialCooldown => 10f;

    protected override bool IsRoundLocked => MatchRounds.IsFirstRoundLocked(OptionGroupSingleton<SheriffOptions>.Instance.FirstRoundUse);

    protected override bool IsCommsLocked => OptionGroupSingleton<SheriffOptions>.Instance.CommsDisables && CommsState.IsActive;

    protected override bool CanTarget(SheriffRole role)
    {
        var options = OptionGroupSingleton<SheriffOptions>.Instance;
        return !role.HasMisfired && !IsRoundLocked && !IsCommsLocked;
    }

    protected override void OnClick()
    {
        var shooter = PlayerControl.LocalPlayer;
        if (Target == null || Target.Data?.Role == null || shooter.Data.Role is not SheriffRole)
        {
            return;
        }

        var (faction, alignmentId) = RoleClassification.Classify(Target.Data.Role);
        var settings = OptionGroupSingleton<SheriffOptions>.Instance.ToShotSettings();
        var outcome = SheriffRules.Decide(faction, alignmentId, settings);

        if (outcome.IsMisfire)
        {
            // Applies here first and then reaches every other client: the match rules need to know on all of them.
            SheriffRpcs.RpcSheriffMisfired(shooter);
        }

        // Order matters: the target is killed while the Sheriff is still alive, then the Sheriff (if the setting says so).
        if (outcome.TargetDies)
        {
            shooter.RpcCustomMurder(Target, MeetingCheck.OutsideMeeting);
        }

        if (outcome.SheriffDies)
        {
            shooter.RpcCustomMurder(shooter, MeetingCheck.OutsideMeeting);
        }

        if (outcome.TargetDies && !OptionGroupSingleton<SheriffOptions>.Instance.CanSelfReport)
        {
            UnreportableBodies.Start(Target.PlayerId);
        }

        if (outcome.IsMisfire)
        {
            Helpers.CreateAndShowNotification(MiraLocaleManager.Get("Eclipse.Role.Sheriff.MisfireFeedback"), Color.white);
        }

        ResetTarget();
    }
}
