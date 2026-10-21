namespace Eclipse.Core;

internal enum FortifyView
{
    Self = 0,
    Warden = 1,
    SelfAndWarden = 2,
    Everyone = 3,
}

internal static class ProtectionRules
{
    public static bool ShowsFortify(FortifyView view, bool viewerIsFortified, bool viewerIsWarden, bool viewerIsDead)
    {
        return viewerIsDead
               || view == FortifyView.Everyone
               || (viewerIsFortified && view is FortifyView.Self or FortifyView.SelfAndWarden)
               || (viewerIsWarden && view is FortifyView.Warden or FortifyView.SelfAndWarden);
    }

    public static bool ShowsBarrier(bool protectedSeesBarrier, bool viewerIsProtected, bool viewerIsCleric, bool viewerIsDead)
    {
        return viewerIsDead || viewerIsCleric || (viewerIsProtected && protectedSeesBarrier);
    }

    public static bool NotifiesProtector(ProtectionKind kind, bool barrierAttackNotification)
    {
        return kind != ProtectionKind.Barrier || barrierAttackNotification;
    }

    public static bool CanFortify(bool alreadyFortifying)
    {
        return !alreadyFortifying;
    }
}
