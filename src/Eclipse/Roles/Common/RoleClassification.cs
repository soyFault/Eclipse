using Eclipse.Api.Factions;
using Eclipse.Api.Roles;
using MiraAPI.Roles;

namespace Eclipse.Roles;

internal static class RoleClassification
{
    public static (Faction Faction, string? AlignmentId) Classify(RoleBehaviour role)
    {
        if (role is IEclipseRole eclipseRole)
        {
            return (eclipseRole.Alignment.Faction, eclipseRole.Alignment.Id);
        }

        if (role is ICustomRole customRole)
        {
            var faction = customRole.Team switch
            {
                ModdedRoleTeams.Crewmate => Faction.Crewmate,
                ModdedRoleTeams.Impostor => Faction.Impostor,
                _ => Faction.Neutral,
            };
            return (faction, null);
        }

        return (role.IsImpostor ? Faction.Impostor : Faction.Crewmate, null);
    }
}
