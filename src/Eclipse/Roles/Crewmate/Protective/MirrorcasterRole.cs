using System.Text;
using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Protective;

internal sealed class MirrorcasterRole : CrewmateRole, IEclipseRole, IPreventsGameEnd, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Mirrorcaster";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Protective;

    public Color RoleColor => EclipseColors.Mirrorcaster;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewProtective;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    // How many Unleash are waiting. Every game keeps the count: the match rules read it everywhere.
    public int UnleashesAvailable { get; set; }

    public bool PreventsGameEnd => Player != null && MirrorcasterRules.PreventsGameEnd(UnleashesAvailable, Mirrors.HasMirrorOf(Player.PlayerId));

    public override bool IsAffectedByComms => false;

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        UnleashesAvailable = 0;
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        Mirrors.RemoveOf(targetPlayer.PlayerId);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        if (Player != null && Mirrors.ProtectedBy(Player.PlayerId) is { } id && GameData.Instance.GetPlayerById(id) is { } protectedPlayer)
        {
            text.AppendLine($"\n<b>{MiraLocaleManager.Get("Eclipse.Role.Mirrorcaster.TabProtecting").Replace("<player>", protectedPlayer.PlayerName)}</b>");
        }

        return text;
    }
}
