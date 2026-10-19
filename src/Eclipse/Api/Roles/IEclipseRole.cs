using Eclipse.Api.Alignments;
using Eclipse.Api.Factions;
using MiraAPI.Roles;

namespace Eclipse.Api.Roles;

/// <summary>
///
/// </summary>
public interface IEclipseRole : ICustomRole
{
    /// <summary>
    ///
    /// </summary>
    AlignmentDefinition Alignment { get; }

    ModdedRoleTeams ICustomRole.Team => Alignment.Faction.ToModdedTeam();

    RoleOptionsGroup ICustomRole.RoleOptionsGroup => Alignment.OptionsGroup;

    string ICustomRole.RoleFactionTitle => Alignment.DisplayName;
}
