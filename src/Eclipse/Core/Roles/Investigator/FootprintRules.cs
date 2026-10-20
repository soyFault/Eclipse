namespace Eclipse.Core;

internal enum FootprintMode
{
    Distance = 0,

    Time = 1,
}

internal static class FootprintRules
{
    public static bool ShouldPrint(FootprintMode mode, float moved, float elapsed, float interval)
    {
        return mode == FootprintMode.Distance ? moved >= interval : elapsed >= interval;
    }
}
