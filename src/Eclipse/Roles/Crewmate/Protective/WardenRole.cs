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

internal sealed class WardenRole : CrewmateRole, IEclipseRole, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Warden";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Protective;

    public Color RoleColor => EclipseColors.Warden;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewProtective;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    public override bool IsAffectedByComms => false;

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        Protections.Ledger.RemoveProtector(targetPlayer.PlayerId, ProtectionKind.Fortify);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        var fortified = Protections.Ledger.TargetsOf(ProtectionKind.Fortify, Player.PlayerId, Protections.Now);
        var name = fortified.Count > 0 ? GameData.Instance.GetPlayerById(fortified[0])?.PlayerName : null;
        if (name != null)
        {
            text.AppendLine($"\n<b>{MiraLocaleManager.Get("Eclipse.Role.Warden.TabProtecting").Replace("<player>", name)}</b>");
        }

        return text;
    }
}
