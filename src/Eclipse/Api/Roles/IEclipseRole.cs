using Eclipse.Api.Alignments;
using Eclipse.Api.Factions;
using MiraAPI.Roles;

namespace Eclipse.Api.Roles;

public interface IEclipseRole : ICustomRole
{
    AlignmentDefinition Alignment { get; }

    ModdedRoleTeams ICustomRole.Team => Alignment.Faction.ToModdedTeam();

    RoleOptionsGroup ICustomRole.RoleOptionsGroup => Alignment.OptionsGroup;

    string ICustomRole.RoleFactionTitle => Alignment.DisplayName;

    TeamIntroConfiguration? ICustomRole.IntroConfiguration => Alignment.Faction == Faction.Neutral
        ? new TeamIntroConfiguration(FactionColors.Neutral, "Eclipse.Intro.Neutral.Title", "Eclipse.Intro.Neutral.Description")
        : null;
}
