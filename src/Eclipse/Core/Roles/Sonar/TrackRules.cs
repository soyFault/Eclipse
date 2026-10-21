namespace Eclipse.Core;

internal static class TrackRules
{
    public static bool UpdateDue(float secondsSinceUpdate, float interval)
    {
        // With an interval of 0 this is always true (the time since the last update is never negative): "every tick".
        return secondsSinceUpdate >= interval;
    }

    public static bool ShouldDrop(bool exists, bool dead, bool disconnected)
    {
        return !exists || dead || disconnected;
    }
}
