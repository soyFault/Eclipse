namespace Eclipse.Core;

internal enum MirrorWhoIsTold
{
    Mirrorcaster = 0,
    MirrorcasterAndKiller = 1,
}

internal enum MirrorAttackInfo
{
    Role = 0,
    Subalignment = 1,
    Faction = 2,
    Nothing = 3,
}

internal enum MirrorMessage
{
    WithoutType = 0,
    WithRole = 1,
    WithFaction = 2,
    WithSubalignment = 3,
}

internal static class MirrorcasterRules
{
    // An attack on a mirrored player is stopped, except: the attacker is the player themselves, the attack ignores defenses (a bomb), the attacker is
    // a Veteran on alert (their answer must not be reflected) or a meeting is going on.
    public static bool Stops(bool targetHasMirror, bool attackerIsTarget, bool ignoresDefense, bool attackerOnAlert, bool inMeetingOrExile)
    {
        return targetHasMirror && !attackerIsTarget && !ignoresDefense && !attackerOnAlert && !inMeetingOrExile;
    }

    public static bool CanPlace(bool alreadyProtecting, int unleashesAvailable, bool accumulate, bool effectActive, float cooldownLeft)
    {
        return !alreadyProtecting && (accumulate || unleashesAvailable <= 0) && !effectActive && cooldownLeft <= 0f;
    }

    public static bool CanUnleashAt(int unleashesAvailable, bool targetIsOwnMirror)
    {
        return unleashesAvailable > 0 && !targetIsOwnMirror;
    }

    // Which message the Mirrorcaster reads, given what the setting says and whether the attacker's role is known.
    public static MirrorMessage MessageFor(MirrorAttackInfo info, bool attackerRoleKnown)
    {
        return info switch
        {
            MirrorAttackInfo.Role when attackerRoleKnown => MirrorMessage.WithRole,
            MirrorAttackInfo.Subalignment when attackerRoleKnown => MirrorMessage.WithSubalignment,
            MirrorAttackInfo.Faction => MirrorMessage.WithFaction,
            _ => MirrorMessage.WithoutType,
        };
    }

    public static bool PreventsGameEnd(int unleashesAvailable, bool mirrorUp)
    {
        return unleashesAvailable > 0 || mirrorUp;
    }

    // The mirror (the drawing and the mark on the name) is seen by the Mirrorcaster and by the dead.
    public static bool ShowsMirror(bool viewerIsMirrorcaster, bool viewerIsDead)
    {
        return viewerIsMirrorcaster || viewerIsDead;
    }
}
