namespace Eclipse.Core;

internal enum BecomeOption
{
    Crew = 0,
    Amnesiac = 1,
    Survivor = 2,
    Mercenary = 3,
    Jester = 4,
}

internal static class BecomeRules
{
    // The id of the role (its IdPart); null for a plain crewmate.
    public static string? RoleIdOf(BecomeOption option)
    {
        return option switch
        {
            BecomeOption.Amnesiac => "Amnesiac",
            BecomeOption.Survivor => "Survivor",
            BecomeOption.Mercenary => "Mercenary",
            BecomeOption.Jester => "Jester",
            _ => null,
        };
    }
}
