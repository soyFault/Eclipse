namespace Eclipse.Core;

internal enum SkipState
{
    No = 0,

    Emergency = 1,

    Always = 2,
}

internal static class SkipButtonRules
{
    public static bool Hides(SkipState state, bool emergencyMeeting)
    {
        return state == SkipState.Always || (state == SkipState.Emergency && emergencyMeeting);
    }
}
