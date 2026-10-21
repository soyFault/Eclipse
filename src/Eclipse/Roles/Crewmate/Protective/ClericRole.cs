using System.Linq;
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

internal sealed class ClericRole : CrewmateRole, IEclipseRole, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Cleric";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Protective;

    public Color RoleColor => EclipseColors.Cleric;

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
        Protections.Ledger.RemoveProtector(targetPlayer.PlayerId, ProtectionKind.Barrier);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        var barriered = Protections.Ledger.TargetsOf(ProtectionKind.Barrier, Player.PlayerId, Protections.Now);
        var name = barriered.Count > 0 ? GameData.Instance.GetPlayerById(barriered.Last())?.PlayerName : null;
        if (name != null)
        {
            text.AppendLine($"\n<b>{MiraLocaleManager.Get("Eclipse.Role.Cleric.TabProtecting").Replace("<player>", name)}</b>");
        }

        return text;
    }
}
