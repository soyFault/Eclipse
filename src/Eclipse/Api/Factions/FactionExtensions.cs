using MiraAPI.Roles;

namespace Eclipse.Api.Factions;

public static class FactionExtensions
{
    public static ModdedRoleTeams ToModdedTeam(this Faction faction)
    {
        return faction switch
        {
            Faction.Crewmate => ModdedRoleTeams.Crewmate,
            Faction.Impostor => ModdedRoleTeams.Impostor,
            _ => ModdedRoleTeams.Custom,
        };
    }
}
