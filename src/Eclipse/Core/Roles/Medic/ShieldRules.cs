namespace Eclipse.Core;

internal enum ShieldNotification
{
    Medic,
    Shielded,
    ShieldedAndMedic,
    Nobody,
    Everyone,
}
internal static class ShieldRules
{
    public static bool Blocks(bool targetIsShielded, byte attackerId, byte targetId, bool ignoresDefense, bool inMeetingOrExile)
    {
        return targetIsShielded && attackerId != targetId && !ignoresDefense && !inMeetingOrExile;
    }

    public static bool GetsAttackNotification(ShieldNotification setting, bool viewerIsMedic, bool viewerIsShielded)
    {
        return setting switch
        {
            ShieldNotification.Medic => viewerIsMedic,
            ShieldNotification.Shielded => viewerIsShielded,
            ShieldNotification.ShieldedAndMedic => viewerIsMedic || viewerIsShielded,
            ShieldNotification.Everyone => true,
            _ => false,
        };
    }
}
