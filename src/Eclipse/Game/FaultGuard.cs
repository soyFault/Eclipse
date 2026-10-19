using System;
using System.Collections.Generic;
using Reactor.Utilities;

namespace Eclipse.Game;

internal static class FaultGuard
{
    private static readonly HashSet<string> Reported = [];
    public static void Run(string what, Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            if (Reported.Add($"{what}|{exception.GetType().Name}|{exception.Message}"))
            {
                Logger<EclipsePlugin>.Error($"{what} failed (shown once; it will keep failing silently until fixed): {exception}");
            }
        }
    }
}
