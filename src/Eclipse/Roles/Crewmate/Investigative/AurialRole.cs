using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Investigative;

internal sealed class AurialRole : CrewmateRole, IEclipseRole
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Aurial";
    public Color RoleColor => EclipseColors.Aurial;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewInvestigative;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };
}
