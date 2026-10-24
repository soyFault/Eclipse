namespace Eclipse.Core;

internal enum FreeplayAction : byte
{
    SetRole = 1,

    ToggleModifier = 2,

    RemoteKill = 3,

    Reset = 4,

    Revive = 5,
}

internal static class MultiplayerFreeplayRules
{
    public static bool Enabled(bool optionOn, bool inFreeplay, bool matchStarted)
    {
        return optionOn && !inFreeplay && matchStarted;
    }

    public static bool MayUse(bool enabled, bool hostOnly, bool amHost)
    {
        return enabled && (amHost || !hostOnly);
    }

    public static bool HostAccepts(bool amHost, bool enabled, bool hostOnly, bool senderIsHost, bool senderConnected)
    {
        return amHost && enabled && senderConnected && (senderIsHost || !hostOnly);
    }

    public static bool IsKnownAction(byte raw)
    {
        return raw is >= (byte)FreeplayAction.SetRole and <= (byte)FreeplayAction.Revive;
    }

    public static bool PreventsGameEnd(bool enabled, bool noEnd)
    {
        return enabled && noEnd;
    }
}
