using AmongUs.GameOptions;
using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Options.Roles.Neutral;
using Eclipse.Roles;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using UnityEngine;

namespace Eclipse.Roles.Neutral;

internal sealed class JesterRole : RoleBehaviour, IEclipseRole, IWinsAlone, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Jester";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Trickster;

    public Color RoleColor => EclipseColors.Jester;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.NeutralEvil;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
        IntroSound = EclipseAssets.IntroSound(IdPart, Alignment.Faction),
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
    };

    // A neutral is alive: the base class does not say so when a role does not derive from CrewmateRole or ImpostorRole.
    public override bool IsDead => false;

    public override bool IsAffectedByComms => false;

    // The win is not kept in the role (a dead Jester has a ghost role instead): it is on the board of NeutralWinners.
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return false;
    }

    // The tasks are fake: the Jester cannot use the consoles that only crewmates use.
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>();
        return console == null || console.AllowImpostor;
    }

    // The default hint talks about finishing tasks; the Jester's goal is in the role tab.
    public override void AppendTaskHint(Il2CppSystem.Text.StringBuilder taskStringBuilder)
    {
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        NeutralTaskHeader.Show(playerControl, Alignment.Id, true);
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        NeutralTaskHeader.Clear(targetPlayer);
    }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);

        if (!OptionGroupSingleton<JesterOptions>.Instance.CanButton)
        {
            player.RemainingEmergencies = 0;
        }
    }
}
