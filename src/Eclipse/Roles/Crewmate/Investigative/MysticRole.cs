using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Colors;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Investigative;

internal sealed class MysticRole : CrewmateRole, IEclipseRole, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Mystic";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Perception;

    public Color RoleColor => EclipseColors.Mystic;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewInvestigative;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };
}
