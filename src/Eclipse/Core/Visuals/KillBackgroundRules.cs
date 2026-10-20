using Eclipse.Api.Factions;

namespace Eclipse.Core;

internal static class KillBackgroundRules
{
    public const string Crewmate = "KillBackgroundCrew";

    public static string? For(Faction? killer)
    {
        return killer switch
        {
            Faction.Crewmate => Crewmate,
            _ => null,
        };
    }
}
