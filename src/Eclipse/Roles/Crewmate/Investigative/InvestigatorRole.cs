using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Investigative;

internal sealed class InvestigatorRole : CrewmateRole, IEclipseRole, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Investigator";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Hunter;

    public Color RoleColor => EclipseColors.Investigator;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewInvestigative;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };
}
