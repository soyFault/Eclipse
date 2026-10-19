using MiraAPI.Roles;
using UnityEngine;

namespace Eclipse.Roles.Crewmate;

internal sealed class AurialRole : CrewmateRole, ICustomRole
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Aurial";

    public Color RoleColor => new Color32(120, 200, 255, 255);

    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new(this);
}
