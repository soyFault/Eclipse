using System;

namespace Eclipse.Core;

internal enum AutoRejoinMode
{
    Never = 0,

    Always = 1,

    Host = 2,

    Client = 3,
}

internal static class AutoRejoinRules
{

    public static bool WillRejoin(AutoRejoinMode mode, bool amHost, bool inFreeplay)
    {
        if (inFreeplay)
        {
            return false;
        }

        return mode switch
        {
            AutoRejoinMode.Always => true,
            AutoRejoinMode.Host => amHost,
            AutoRejoinMode.Client => !amHost,
            _ => false,
        };
    }

    public static int SecondsToShow(float secondsLeft)
    {
        return Math.Max(1, (int)Math.Ceiling(secondsLeft));
    }
}
