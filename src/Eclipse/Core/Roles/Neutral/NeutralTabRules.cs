namespace Eclipse.Core;

internal enum NeutralTab
{
    Benign = 0,

    Evil = 1,

    Killing = 2,

    Outlier = 3,

    Won = 4,

    DeadWon = 5,

    Dead = 6,
}

internal static class NeutralTabRules
{
    public static NeutralTab Choose(bool alive, bool won, string? alignmentId)
    {
        if (!alive)
        {
            return won ? NeutralTab.DeadWon : NeutralTab.Dead;
        }

        if (won)
        {
            return NeutralTab.Won;
        }

        return alignmentId switch
        {
            AlignmentIds.NeutralBenign => NeutralTab.Benign,
            AlignmentIds.NeutralKilling => NeutralTab.Killing,
            AlignmentIds.NeutralOutlier => NeutralTab.Outlier,
            _ => NeutralTab.Evil,
        };
    }

    public static string KeyOf(NeutralTab tab)
    {
        return $"Eclipse.TabText.{tab}";
    }

    public static bool UsesTeamColor(NeutralTab tab)
    {
        return tab is not (NeutralTab.Dead or NeutralTab.DeadWon);
    }
}
